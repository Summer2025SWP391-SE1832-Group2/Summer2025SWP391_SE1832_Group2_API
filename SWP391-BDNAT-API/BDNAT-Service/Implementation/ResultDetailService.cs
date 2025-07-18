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

namespace BDNAT_Service.Implementation
{
    public class ResultDetailService : IResultDetailService
    {
        private readonly IMapper _mapper;
        private readonly PaternityCalculationService _paternityCalculationService;
        public ResultDetailService(IMapper mapper, PaternityCalculationService paternityCalculationService)
        {
            _paternityCalculationService = paternityCalculationService;

            _mapper = mapper;
        }

        public async Task<bool> CreateResultAsync(ResultDetailDTO result)
        {
            var map = _mapper.Map<ResultDetail>(result);
            return await ResultDetailRepo.Instance.InsertAsync(map);
        }

        /*public async Task<bool> CreateMultipleResultsAsync(SaveResultDetailRequest dto)
        {
            var updateBooking = await BookingRepo.Instance.GetById(dto.BookingId);
            updateBooking.FinalResult = dto.FinalResult;
            updateBooking.Status = "Hoàn thành";
            var check = await BookingRepo.Instance.UpdateAsync(updateBooking);
            if (check)
            {
                if (dto.Results == null || !dto.Results.Any())
                    return false;

                var resultEntities = dto.Results.Select(r => new ResultDetail
                {
                    BookingId = dto.BookingId,
                    TestParameterId = r.TestParameterId ?? 0,
                    Value = r.Value,
                    SampleId = r.SampleId ?? 0
                }).ToList();

                await ResultDetailRepo.Instance.AddRangeAsync(resultEntities);
                return true;
            }
            return false;
        }*/

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
                // Lấy danh sách result hiện tại trong DB
                var existingResults = await ResultDetailRepo.Instance.GetResultDetailsByBookingIdAsync(dto.BookingId);

                foreach (var updateItem in dto.Results)
                {
                    var result = existingResults.FirstOrDefault(r => r.ResultDetailId == updateItem.ResultDetailId);
                    if (result != null)
                    {
                        result.Value = updateItem.Value;
                        result.TestParameterId = updateItem.TestParameterId ?? result.TestParameterId;
                        result.SampleId = updateItem.SampleId ?? result.SampleId;
                    }
                }

                // Cập nhật batch
                var updated = await ResultDetailRepo.Instance.UpdateRangeAsync(existingResults);
                if (!updated)
                    return false;

                // Lấy lại thông tin booking
                var booking = await BookingRepo.Instance.GetById(dto.BookingId);
                if (booking == null) return false;

                // Kiểm tra có phải NIPT không
                bool isNipt = booking.Service?.Name != null &&
                              booking.Service.Name.Contains("NIPT", StringComparison.OrdinalIgnoreCase);

                string calculatedResult = dto.FinalResult;

                // Nếu không phải NIPT, tính toán lại kết quả cha-con
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

                // Cập nhật Booking
                booking.FinalResult = calculatedResult;
                booking.Status = "Hoàn thành";

                return await BookingRepo.Instance.UpdateAsync(booking);
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

            try
            {
                bool isNipt = updateBooking.Service?.Name != null &&
                              updateBooking.Service.Name.Contains("NIPT", StringComparison.OrdinalIgnoreCase);

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
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi trong CreateMultipleResultsAsync: {ex.Message}");
                return false;
            }
        }

        public string GenerateNiptConclusion(SaveResultDetailRequest dto)
        {
            string cfDnaConclusionNote = string.Empty;
            string result = string.Empty;

            // Parse cfDNA value
            if (!double.TryParse(dto.cfDNA, out var fetalFraction))
            {
                return "Không thể xác định được cfDNA hợp lệ.";
            }

            // Nếu cfDNA < 4% thì không xác định được
            if (fetalFraction/100 < 4.0)
            {
                return "Fetal Fraction quá thấp (<4%). Không xác định được nguy cơ cho các trisomy.";
            }

            // Dictionary lưu kết luận cho từng loại trisomy
            var conclusions = new Dictionary<string, string>();

            foreach (var trisomy in new[] { "21", "18", "13" })
            {
                var paramName = $"Z-score (Trisomy {trisomy})";
                var resultParam = dto.Results.FirstOrDefault(r => r.ParameterName == paramName);

                if (resultParam != null && double.TryParse(resultParam.Value, out var zScore))
                {
                    if (zScore > 2.5)
                        conclusions[trisomy] = "Nguy cơ cao";
                    else
                        conclusions[trisomy] = "Nguy cơ thấp";
                }
                else
                {
                    conclusions[trisomy] = "Không có dữ liệu";
                }
            }

            // Format kết luận
            result = $"Kết quả NIPT:\n" +
                     $"- Trisomy 21: {conclusions["21"]}\n" +
                     $"- Trisomy 18: {conclusions["18"]}\n" +
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
