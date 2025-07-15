using BDNAT_Repository.DTO;
using BDNAT_Repository.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Implementation
{
    public class AlleleComparison
    {
        public string Locus { get; set; }
        public string[] Person1Alleles { get; set; }
        public string[] Person2Alleles { get; set; }
        public double PI { get; set; }
        public string ComparisonType { get; set; }
        public string Formula { get; set; }
    }

    public class PaternityResult
    {
        public List<AlleleComparison> Comparisons { get; set; }
        public double CPI { get; set; }
        public double W { get; set; }
        public string Conclusion { get; set; }

        public PaternityResult()
        {
            Comparisons = new List<AlleleComparison>();
        }
    }

    public class PaternityCalculationService
    {
        private readonly AlleleDataService _alleleDataService;

        public PaternityCalculationService(AlleleDataService alleleDataService)
        {
            _alleleDataService = alleleDataService;
        }

        // Phương thức chính để tính toán từ ResultDetailDTO
        public PaternityResult CalculateFromResultDetails(List<ResultDetailDTO> resultDetails)
        {
            var sampleData = GroupResultsBySample(resultDetails);

            if (sampleData.Count < 2)
            {
                throw new InvalidOperationException("Cần ít nhất 2 mẫu để tính toán quan hệ cha con");
            }

            var samples = sampleData.Keys.ToList();
            var person1Data = sampleData[samples[0]];
            var person2Data = sampleData[samples[1]];

            return CalculatePaternity(person1Data, person2Data);
        }

        private Dictionary<int, Dictionary<string, string[]>> GroupResultsBySample(List<ResultDetailDTO> resultDetails)
        {
            var sampleData = new Dictionary<int, Dictionary<string, string[]>>();

            foreach (var detail in resultDetails)
            {
                if (!detail.SampleId.HasValue || string.IsNullOrEmpty(detail.ParameterName) || string.IsNullOrEmpty(detail.Value))
                    continue;

                var sampleId = detail.SampleId.Value;
                var parameterName = detail.ParameterName;
                var alleles = ParseAlleles(detail.Value);

                if (!sampleData.ContainsKey(sampleId))
                {
                    sampleData[sampleId] = new Dictionary<string, string[]>();
                }

                sampleData[sampleId][parameterName] = alleles;
            }

            return sampleData;
        }

        public string GeneratePaternityReport(PaternityResult result, List<ResultDetailDTO> resultDetails)
        {
            var report = new StringBuilder();
            report.AppendLine("=== BÁO CÁO XÁC ĐỊNH QUAN HỆ CHA CON ===");
            report.AppendLine();

            // Thông tin mẫu
            var sampleNames = resultDetails.Where(r => r.SampleId.HasValue)
                                         .GroupBy(r => r.SampleId.Value)
                                         .ToDictionary(g => g.Key, g => g.First().SampleOwnerName ?? $"Mẫu {g.Key}");

            report.AppendLine("THÔNG TIN MẪU:");
            foreach (var sample in sampleNames)
            {
                report.AppendLine($"- {sample.Value} (ID: {sample.Key})");
            }
            report.AppendLine();

            // Chi tiết so sánh
            report.AppendLine("CHI TIẾT SO SÁNH:");
            report.AppendLine("| Locus | Người 1 | Người 2 | Loại so sánh | PI | Công thức |");
            report.AppendLine("|-------|---------|---------|--------------|----|-----------");

            foreach (var comp in result.Comparisons)
            {
                var person1Str = string.Join("-", comp.Person1Alleles);
                var person2Str = string.Join("-", comp.Person2Alleles);
                report.AppendLine($"| {comp.Locus} | {person1Str} | {person2Str} | {comp.ComparisonType} | {comp.PI:F4} | {comp.Formula} |");
            }

            report.AppendLine();
            report.AppendLine("KẾT QUỦ:");
            report.AppendLine($"- CPI (Combined Paternity Index): {result.CPI:F4}");
            report.AppendLine($"- W (Xác suất quan hệ cha con): {result.W:P4}");
            report.AppendLine($"- Kết luận: {result.Conclusion}");

            return report.ToString();
        }

        public PaternityResult CalculatePaternity(Dictionary<string, string[]> person1Alleles, Dictionary<string, string[]> person2Alleles)
        {
            var result = new PaternityResult();
            var alleleData = _alleleDataService.GetAlleleData();

            foreach (var locus in person1Alleles.Keys)
            {
                // Bỏ qua locus AMEL (giới tính)
                if (locus.ToUpper().Contains("AMEL"))
                    continue;

                if (person2Alleles.ContainsKey(locus))
                {
                    var comparison = CompareAlleles(locus, person1Alleles[locus], person2Alleles[locus], alleleData);
                    if (comparison != null)
                    {
                        result.Comparisons.Add(comparison);
                    }
                }
            }

            // Tính CPI (Combined Paternity Index)
            result.CPI = result.Comparisons.Where(c => c.PI >= 0).Aggregate(1.0, (acc, c) => acc * c.PI);

            // Tính W (Xác suất quan hệ cha con)
            result.W = (result.CPI * 0.5) / (result.CPI * 0.5 + 0.5);

            // Kết luận
            result.Conclusion = GetConclusion(result.W, result.CPI);

            return result;
        }

        private AlleleComparison CompareAlleles(string locus, string[] person1Alleles, string[] person2Alleles, List<AlleleData> alleleData)
        {
            var comparison = new AlleleComparison
            {
                Locus = locus,
                Person1Alleles = person1Alleles,
                Person2Alleles = person2Alleles
            };

            // Tìm allele chung
            var commonAlleles = person1Alleles.Intersect(person2Alleles).ToArray();

            if (commonAlleles.Length == 0)
            {
                // Trường hợp không giống nhau => PI = 0
                comparison.PI = 0;
                comparison.ComparisonType = "Không giống nhau";
                comparison.Formula = "1/0 = 0";
                return comparison;
            }

            // Lấy tần suất từ dữ liệu
            var frequencies = GetAlleleFrequencies(commonAlleles, locus, alleleData);

            if (frequencies.Count == 0)
            {
                // Không tìm thấy tần suất trong dữ liệu
                comparison.PI = 0;
                comparison.ComparisonType = "Không có dữ liệu tần suất";
                comparison.Formula = "Không có dữ liệu";
                return comparison;
            }

            // Xác định trường hợp
            if (person1Alleles.Length == 2 && person1Alleles[0] == person1Alleles[1] &&
                person2Alleles.Length == 2 && person2Alleles[0] == person2Alleles[1] &&
                person1Alleles[0] == person2Alleles[0])
            {
                // Trường hợp 2 cặp giống nhau => PI = 1/(q+p)
                var q = frequencies.First().Value;
                var p = frequencies.First().Value; // Cùng allele nên p = q
                comparison.PI = 1 / (q + p);
                comparison.ComparisonType = "2 cặp giống nhau";
                comparison.Formula = $"1/({q:F4}+{p:F4}) = {comparison.PI:F4}";
            }
            else if (commonAlleles.Length == 1)
            {
                // Trường hợp 1 cặp giống nhau => PI = 1/q
                var q = frequencies.First().Value;
                comparison.PI = 1 / q;
                comparison.ComparisonType = "1 cặp giống nhau";
                comparison.Formula = $"1/{q:F4} = {comparison.PI:F4}";
            }
            else
            {
                // Trường hợp có nhiều allele chung => PI = 1/(q+p)
                var sortedFreqs = frequencies.Values.OrderBy(f => f).ToArray();
                var q = sortedFreqs[0];
                var p = sortedFreqs.Length > 1 ? sortedFreqs[1] : sortedFreqs[0];
                comparison.PI = 1 / (q + p);
                comparison.ComparisonType = "Nhiều allele chung";
                comparison.Formula = $"1/({q:F4}+{p:F4}) = {comparison.PI:F4}";
            }

            return comparison;
        }

        private Dictionary<string, double> GetAlleleFrequencies(string[] alleles, string locus, List<AlleleData> alleleData)
        {
            var frequencies = new Dictionary<string, double>();

            foreach (var allele in alleles)
            {
                var alleleRecord = alleleData.FirstOrDefault(a => a.Allele == allele);
                if (alleleRecord != null && alleleRecord.MarkerValues.ContainsKey(locus))
                {
                    var frequency = alleleRecord.MarkerValues[locus];
                    if (frequency.HasValue && frequency.Value > 0)
                    {
                        frequencies[allele] = frequency.Value;
                    }
                }
            }

            return frequencies;
        }

        private string GetConclusion(double w, double cpi)
        {
            if (cpi == 0)
                return "Loại trừ - Không phải là cha";
            else if (w >= 0.9999)
                return "Rất chắc chắn là cha";
            else if (w >= 0.999)
                return "Chắc chắn là cha";
            else if (w >= 0.99)
                return "Có khả năng cao là cha";
            else if (w >= 0.9)
                return "Có khả năng là cha";
            else
                return "Không đủ bằng chứng";
        }

        // Phương thức helper để parse input string thành array
        public static string[] ParseAlleles(string alleleString)
        {
            if (string.IsNullOrEmpty(alleleString))
                return new string[0];

            // Xử lý các format khác nhau: "11-16", "11,16", "11 16"
            var separators = new char[] { '-', ',', ' ' };
            return alleleString.Split(separators, StringSplitOptions.RemoveEmptyEntries)
                              .Select(s => s.Trim())
                              .ToArray();
        }

        // Phương thức để tạo input data từ string
        public static Dictionary<string, string[]> CreatePersonData(Dictionary<string, string> rawData)
        {
            var result = new Dictionary<string, string[]>();

            foreach (var kvp in rawData)
            {
                result[kvp.Key] = ParseAlleles(kvp.Value);
            }

            return result;
        }
    }
}
