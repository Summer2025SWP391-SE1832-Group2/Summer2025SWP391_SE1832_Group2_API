using AutoMapper;
using BDNAT_Repository.Entities;
using BDNAT_Repository.Implementation;
using BDNAT_Repository.DTO;
using BDNAT_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Globalization;
using Microsoft.IdentityModel.Tokens;
using Azure.Core;

namespace BDNAT_Service.Implementation
{
    public class ResultDetailService : IResultDetailService
    {
        private readonly IMapper _mapper;
        private readonly PaternityCalculationService _paternityCalculationService;
        private readonly FirebaseNotificationService _fcmService;
        private readonly INotificationService _notificationService;
        public ResultDetailService(IMapper mapper, PaternityCalculationService paternityCalculationService, INotificationService notificationService)
        {
            _paternityCalculationService = paternityCalculationService;

            _mapper = mapper;

            _fcmService = new FirebaseNotificationService();
            _notificationService = notificationService;
        }

        public async Task<bool> CreateResultAsync(ResultDetailDTO result)
        {
            var map = _mapper.Map<ResultDetail>(result);
            return await ResultDetailRepo.Instance.InsertAsync(map);
        }

        public async Task<bool> DeleteResultAsync(int id)
        {
            return await ResultDetailRepo.Instance.DeleteAsync(id);
        }

        public async Task<List<ResultDetailDTO>> GetAllResultsAsync()
        {
            var list = await ResultDetailRepo.Instance.GetAllAsync();
            return list.Select(x => _mapper.Map<ResultDetailDTO>(x)).ToList();
        }

        public async Task<ResultDetailDTO> GetResultByIdAsync(int id)
        {
            return _mapper.Map<ResultDetailDTO>(await ResultDetailRepo.Instance.GetByIdAsync(id));
        }

        public async Task<List<ResultDetailDTO>> GetResultDetailsByBookingIdAsync(int BookingId)
        {
            var list = await ResultDetailRepo.Instance.GetResultDetailsByBookingIdAsync(BookingId);
            return list.Select(x => _mapper.Map<ResultDetailDTO>(x)).ToList();
        }

        public async Task<bool> DeleteBySampleIdAsync(int sampleId)
        {
            return await ResultDetailRepo.Instance.DeleteWhereAsync(r => r.SampleId == sampleId);
        }

        public async Task<bool> DeleteByBookingIdAsync(int bookingId)
        {
            return await ResultDetailRepo.Instance.DeleteWhereAsync(r => r.BookingId == bookingId);
        }

        public async Task<bool> UpdateResultAsync(ResultDetailDTO result)
        {
            var map = _mapper.Map<ResultDetail>(result);
            return await ResultDetailRepo.Instance.UpdateAsync(map);
        }

        public async Task<bool> UpdateMultipleResultsAsync(SaveResultDetailRequest dto)
        {
            if (dto == null || dto.BookingId == 0 || dto.Results == null || !dto.Results.Any())
                return false;

            try
            {
                var updateBooking = await BookingRepo.Instance.GetById(dto.BookingId);
                if (updateBooking == null) return false;

                var service = await ServiceRepo.Instance.GetById(updateBooking.ServiceId);
                if (service == null) return false;

                bool isNipt = service.Name != null &&
                              service.Name.Contains("NIPT", StringComparison.OrdinalIgnoreCase);

                string calculatedResult = dto.FinalResult;

                // 1. Tính kết quả nếu là NIPT
                if (isNipt)
                {
                    calculatedResult = GenerateNiptConclusion(dto);
                }
                else
                {
                    var isDnaPaternityTest = CheckIfDnaPaternityTest(dto.Results);

                    if (isDnaPaternityTest)
                    {
                        try
                        {
                            var paternityResult = _paternityCalculationService.CalculateFromResultDetails(dto.Results);
                            var detailedReport = _paternityCalculationService.GeneratePaternityReport(paternityResult, dto.Results);

                            calculatedResult = paternityResult.W * 100 + "% " + paternityResult.Conclusion;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Lỗi khi tính toán xác suất cha con: {ex.Message}");
                            calculatedResult = dto.FinalResult ?? "Không thể tính toán được kết quả";
                        }
                    }
                }

                // 2. Cập nhật kết quả cuối và trạng thái cho booking
                updateBooking.FinalResult = calculatedResult;
                updateBooking.Status = "Hoàn thành";
                var bookingUpdated = await BookingRepo.Instance.UpdateAsync(updateBooking);

                if (!bookingUpdated)
                    return false;

                // 3. Lấy result cũ trong DB
                var existingResults = await ResultDetailRepo.Instance.GetResultDetailsByBookingIdAsync(dto.BookingId);
                if (existingResults == null || !existingResults.Any())
                    return false;

                // 4. Nếu không phải NIPT, tính PI để cập nhật vào ResultDetail
                Dictionary<string, double>? piLookup = null;

                if (!isNipt)
                {
                    var paternityResult = _paternityCalculationService.CalculateFromResultDetails(dto.Results);
                    piLookup = paternityResult.Comparisons.ToDictionary(c => c.Locus, c => c.PI);
                }

                // 5. Cập nhật từng kết quả
                foreach (var updateItem in dto.Results)
                {
                    var result = existingResults.FirstOrDefault(r => r.ResultDetailId == updateItem.ResultDetailId);
                    if (result != null)
                    {
                        result.Value = updateItem.Value;
                        result.TestParameterId = updateItem.TestParameterId ?? result.TestParameterId;
                        result.SampleId = updateItem.SampleId ?? result.SampleId;

                        // Reset PI nếu không có giá trị
                        if (string.IsNullOrWhiteSpace(result.Value))
                            result.Pi = null;

                        // Nếu không phải NIPT và dữ liệu phù hợp thì gán lại PI
                        if (!isNipt &&
                            !string.IsNullOrEmpty(updateItem.ParameterName) &&
                            piLookup != null &&
                            piLookup.ContainsKey(updateItem.ParameterName))
                        {
                            var sampleIds = dto.Results
                                .Where(x => x.ParameterName == updateItem.ParameterName)
                                .Select(x => x.SampleId ?? 0)
                                .Distinct()
                                .OrderBy(x => x)
                                .ToList();

                            if (sampleIds.Count >= 2 && updateItem.SampleId == sampleIds[0])
                            {
                                result.Pi = piLookup[updateItem.ParameterName];
                            }
                        }
                    }
                }

                var checkUpdate = await ResultDetailRepo.Instance.UpdateRangeAsync(existingResults);
                if (!checkUpdate) return false;

                var user = await UserRepo.Instance.GetById(updateBooking.UserId);
                if (user == null) return false;

                var notificationToken = await FirebaseNotificationRepo.Instance.GetLatestValidTokenByUserIdAsync(user.UserId);
                if (string.IsNullOrWhiteSpace(notificationToken?.Token)) return false;

                string title = "Đã cập nhật kết quả mới!";
                string body = "Vui lòng kiểm tra hồ sơ xét nghiệm.Booking ID: " + updateBooking.BookingId;

                await _fcmService.SendNotificationAsync(title, body, notificationToken.Token);

                var notification = new NotificationDTO
                {
                    UserId = updateBooking.UserId,
                    Title = title,
                    Body = body,
                    ReceivedAt = DateTime.UtcNow,
                    IsRead = false
                };

                return await _notificationService.CreateNotificationAsync(notification);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi trong UpdateMultipleResultsAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CreateMultipleResultsAsync(SaveResultDetailRequest dto)
        {
            var updateBooking = await BookingRepo.Instance.GetById(dto.BookingId);
            var service = await ServiceRepo.Instance.GetById(updateBooking.ServiceId);

            try
            {
                bool isNipt = service.Name != null &&
                              service.Name.Contains("NIPT", StringComparison.OrdinalIgnoreCase);

                string calculatedResult = dto.FinalResult;

                if (isNipt)
                {
                    calculatedResult = GenerateNiptConclusion(dto);
                }
                else
                {
                    // Tính toán xác suất quan hệ cha con nếu không phải NIPT
                    if (dto.Results != null && dto.Results.Any())
                    {
                        var isDnaPaternityTest = CheckIfDnaPaternityTest(dto.Results);

                        if (isDnaPaternityTest)
                        {
                            try
                            {
                                var paternityResult = _paternityCalculationService.CalculateFromResultDetails(dto.Results);
                                var detailedReport = _paternityCalculationService.GeneratePaternityReport(paternityResult, dto.Results);

                                calculatedResult = paternityResult.W * 100 + "% " + paternityResult.Conclusion;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Lỗi khi tính toán xác suất cha con: {ex.Message}");
                                calculatedResult = dto.FinalResult ?? "Không thể tính toán được kết quả";
                            }
                        }
                    }
                }

                // Cập nhật trạng thái và kết quả cuối
                updateBooking.FinalResult = calculatedResult;
                updateBooking.Status = "Hoàn thành";

                var check = await BookingRepo.Instance.UpdateAsync(updateBooking);

                if (!check || dto.Results == null || !dto.Results.Any())
                    return false;

                var resultEntities = new List<ResultDetail>();

                Dictionary<string, double>? piLookup = null;

                // Nếu không phải NIPT, thì tính PI để lưu vào ResultDetail
                if (!isNipt)
                {
                    var paternityResult = _paternityCalculationService.CalculateFromResultDetails(dto.Results);
                    piLookup = paternityResult.Comparisons.ToDictionary(c => c.Locus, c => c.PI);
                }

                foreach (var r in dto.Results)
                {
                    var entity = new ResultDetail
                    {
                        BookingId = dto.BookingId,
                        TestParameterId = r.TestParameterId ?? 0,
                        Value = r.Value,
                        SampleId = r.SampleId ?? 0
                    };

                    // Chỉ lưu PI nếu không phải NIPT và dữ liệu phù hợp
                    if (!isNipt &&
                        !string.IsNullOrEmpty(r.ParameterName) &&
                        piLookup != null &&
                        piLookup.ContainsKey(r.ParameterName))
                    {
                        var sampleIds = dto.Results
                                           .Where(x => x.ParameterName == r.ParameterName)
                                           .Select(x => x.SampleId ?? 0)
                                           .Distinct()
                                           .OrderBy(x => x)
                                           .ToList();

                        if (sampleIds.Count >= 2 && r.SampleId == sampleIds[0])
                        {
                            entity.Pi = piLookup[r.ParameterName];
                        }
                    }

                    resultEntities.Add(entity);
                }

                await ResultDetailRepo.Instance.AddRangeAsync(resultEntities);

                var user = await UserRepo.Instance.GetById(updateBooking.UserId);
                if (user == null) return false;

                var notificationToken = await FirebaseNotificationRepo.Instance.GetLatestValidTokenByUserIdAsync(user.UserId);
                if (string.IsNullOrWhiteSpace(notificationToken?.Token)) return false;

                string title = "Đã cập nhật kết quả!";
                string body = "Vui lòng kiểm tra hồ sơ xét nghiệm.Booking ID: " + updateBooking.BookingId;


                await _fcmService.SendNotificationAsync(title, body, notificationToken.Token);

                var notification = new NotificationDTO
                {
                    UserId = updateBooking.UserId,
                    Title = title,
                    Body = body,
                    ReceivedAt = DateTime.UtcNow,
                    IsRead = false
                };

                return await _notificationService.CreateNotificationAsync(notification);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi trong CreateMultipleResultsAsync: {ex.Message}");
                return false;
            }
        }

        public string GenerateNiptConclusion(SaveResultDetailRequest dto)
        {
            string result = string.Empty;

            var cfDnaParam = dto.Results.FirstOrDefault(r =>
                !string.IsNullOrEmpty(r.ParameterName) &&
                r.ParameterName.Trim().Equals("cfDNA", StringComparison.OrdinalIgnoreCase));

            if (cfDnaParam == null || !double.TryParse(cfDnaParam.Value?.Replace("%", "").Trim(), out var fetalFraction))
            {
                return "Không thể xác định được cfDNA hợp lệ.";
            }

            if (fetalFraction < 4.0)
            {
                return "Fetal Fraction quá thấp (<4%). Không xác định được nguy cơ cho các trisomy.";
            }

            var conclusions = new Dictionary<string, string>();

            foreach (var trisomy in new[] { "21", "18", "13" })
            {
                var paramName = $"Trisomy {trisomy}";
                var resultParam = dto.Results.FirstOrDefault(r => r.ParameterName?.Trim() == paramName);

                if (resultParam != null && double.TryParse(resultParam.Value, out var zScore))
                {
                    conclusions[trisomy] = zScore > 2.5 ? "Nguy cơ cao" : "Nguy cơ thấp";
                }
                else
                {
                    conclusions[trisomy] = "Không có dữ liệu";
                }
            }

            result = $"Kết quả NIPT: " +
                     $"- Trisomy 21: {conclusions["21"]} " +
                     $"- Trisomy 18: {conclusions["18"]} " +
                     $"- Trisomy 13: {conclusions["13"]}";

            return result;
        }


        private bool CheckIfDnaPaternityTest(List<ResultDetailDTO> results)
        {
            // Kiểm tra xem có phải test DNA xác định quan hệ cha con không
            // Dựa trên tên parameter hoặc các marker DNA phổ biến
            var dnaMarkers = new HashSet<string>
            {
                "D3S1358", "D1S1656", "D2S441", "D10S1248", "D13S317",
                "PentaE", "D16S539", "D18S51", "D2S1338", "CSF1PO",
                "PentaD", "TH01", "vWA", "D21S11", "D7S820", "D5S818",
                "TPOX", "D8S1179", "D12S391", "FGA", "D22S1045"
            };

            return results.Any(r => !string.IsNullOrEmpty(r.ParameterName) &&
                                   dnaMarkers.Contains(r.ParameterName));
        }

        // Phương thức riêng để chỉ tính toán mà không lưu DB (dùng cho preview)
        public PaternityResult CalculatePaternityPreview(List<ResultDetailDTO> results)
        {
            return _paternityCalculationService.CalculateFromResultDetails(results);
        }

        public async Task<bool> ProcessExcelAndCreateResultsAsync(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets[0];
            var rowCount = worksheet.Dimension.End.Row;

            for (int row = 2; row <= rowCount; row++)
            {
                if (!int.TryParse(worksheet.Cells[row, 1].Text, out int bookingId))
                    continue;

                var request = new SaveResultDetailRequest
                {
                    BookingId = bookingId,
                    FinalResult = worksheet.Cells[row, 2].Text,
                    //cfDNA = worksheet.Cells[row, 3].Text,
                    Results = new List<ResultDetailDTO>
                {
                    new ResultDetailDTO
                    {
                        TestParameterId = int.TryParse(worksheet.Cells[row, 4].Text, out var testParamId) ? testParamId : null,
                        ParameterName = worksheet.Cells[row, 5].Text,
                        Description = worksheet.Cells[row, 6].Text,
                        Value = worksheet.Cells[row, 7].Text,
                        SampleId = int.TryParse(worksheet.Cells[row, 8].Text, out var sampleId) ? sampleId : null,
                        SampleOwnerName = worksheet.Cells[row, 9].Text,
                        Pi = double.TryParse(worksheet.Cells[row, 10].Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var pi) ? pi : null
                    }
                }
                };

                await CreateMultipleResultsAsync(request);
            }

            return true;
        }
    }

    // Extension method để format kết quả đẹp hơn
    public static class PaternityResultExtensions
    {
        public static string ToFormattedString(this PaternityResult result)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"CPI: {result.CPI:F4}");
            sb.AppendLine($"Xác suất: {result.W:P2}");
            sb.AppendLine($"Kết luận: {result.Conclusion}");
            return sb.ToString();
        }  
    }
}
