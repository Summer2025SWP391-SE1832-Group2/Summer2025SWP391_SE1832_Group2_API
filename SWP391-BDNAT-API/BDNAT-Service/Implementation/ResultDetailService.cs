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

        public async Task<bool> UpdateMultipleResultsAsync(SaveResultDetailRequest dto)
        {
            if (dto == null || dto.BookingId == 0 || dto.Results == null || !dto.Results.Any())
                return false;

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

            // Cập nhật Booking
            var booking = await BookingRepo.Instance.GetById(dto.BookingId);
            if (booking == null) return false;

            booking.FinalResult = dto.FinalResult;
            booking.Status = "Hoàn thành";

            return await BookingRepo.Instance.UpdateAsync(booking);
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

        public async Task<bool> CreateMultipleResultsAsync(SaveResultDetailRequest dto)
        {
            var updateBooking = await BookingRepo.Instance.GetById(dto.BookingId);

            try
            {
                // Tính toán xác suất quan hệ cha con nếu có đủ dữ liệu
                string calculatedResult = dto.FinalResult;

                if (dto.Results != null && dto.Results.Any())
                {
                    // Kiểm tra xem có phải test xác định quan hệ cha con không
                    var isDnaPaternityTest = CheckIfDnaPaternityTest(dto.Results);

                    if (isDnaPaternityTest)
                    {
                        try
                        {
                            var paternityResult = _paternityCalculationService.CalculateFromResultDetails(dto.Results);
                            var detailedReport = _paternityCalculationService.GeneratePaternityReport(paternityResult, dto.Results);

                            // Cập nhật kết quả cuối cùng với báo cáo chi tiết
                            calculatedResult = paternityResult.W *100 + "% " + paternityResult.Conclusion;

                            // Hoặc nếu chỉ muốn kết luận ngắn gọn:
                            // calculatedResult = $"CPI: {paternityResult.CPI:F4}, W: {paternityResult.W:P2}, Kết luận: {paternityResult.Conclusion}";
                        }
                        catch (Exception ex)
                        {
                            // Log lỗi nhưng vẫn tiếp tục với kết quả gốc
                            Console.WriteLine($"Lỗi khi tính toán xác suất cha con: {ex.Message}");
                            calculatedResult = dto.FinalResult ?? "Không thể tính toán được kết quả";
                        }
                    }
                }

                // Cập nhật booking
                updateBooking.FinalResult = calculatedResult;
                updateBooking.Status = "Hoàn thành";
                var check = await BookingRepo.Instance.UpdateAsync(updateBooking);

                if (check)
                {
                    if (dto.Results == null || !dto.Results.Any())
                        return false;

                    var resultEntities = new List<ResultDetail>();
                        // Tính toán PI cho từng locus và lưu vào ResultDetail
                        var paternityResult = _paternityCalculationService.CalculateFromResultDetails(dto.Results);
                        var piLookup = paternityResult.Comparisons.ToDictionary(c => c.Locus, c => c.PI);

                        foreach (var r in dto.Results)
                        {
                            var entity = new ResultDetail
                            {
                                BookingId = dto.BookingId,
                                TestParameterId = r.TestParameterId ?? 0,
                                Value = r.Value,
                                SampleId = r.SampleId ?? 0
                            };

                            // Lưu PI vào sample có ID nhỏ hơn trong cặp so sánh
                            if (!string.IsNullOrEmpty(r.ParameterName) && piLookup.ContainsKey(r.ParameterName))
                            {
                                var sampleIds = dto.Results.Where(x => x.ParameterName == r.ParameterName)
                                                          .Select(x => x.SampleId ?? 0)
                                                          .Distinct()
                                                          .OrderBy(x => x)
                                                          .ToList();

                                // Chỉ lưu PI vào sample có ID nhỏ nhất
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
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi trong CreateMultipleResultsAsync: {ex.Message}");
                return false;
            }
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
