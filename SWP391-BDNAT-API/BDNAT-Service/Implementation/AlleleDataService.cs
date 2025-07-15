using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Implementation
{
    public class AlleleData
    {
        public string Allele { get; set; }
        public Dictionary<string, double?> MarkerValues { get; set; }

        public AlleleData()
        {
            MarkerValues = new Dictionary<string, double?>();
        }
    }

    // Service để đọc và xử lý dữ liệu
    public class AlleleDataService
    {
        private static List<AlleleData> _alleleData = null;
        private static readonly object _lock = new object();
        private readonly string _csvFilePath;

        public AlleleDataService(IConfiguration configuration)
        {
            _csvFilePath = configuration["AlleleData:FilePath"] ?? "Data/Allele - Trang tính1.csv";
        }

        public List<AlleleData> GetAlleleData()
        {
            if (_alleleData == null)
            {
                lock (_lock)
                {
                    if (_alleleData == null)
                    {
                        _alleleData = LoadDataFromCsv();
                    }
                }
            }
            return _alleleData;
        }

        private List<AlleleData> LoadDataFromCsv()
        {
            var alleleList = new List<AlleleData>();

            try
            {
                if (!File.Exists(_csvFilePath))
                {
                    throw new FileNotFoundException($"File không tồn tại: {_csvFilePath}");
                }

                var lines = File.ReadAllLines(_csvFilePath);

                if (lines.Length < 2)
                {
                    throw new InvalidOperationException("File CSV không có dữ liệu");
                }

                // Đọc header (các marker)
                var headers = lines[0].Split(',');
                var markers = headers.Skip(1).ToArray(); // Bỏ qua cột "Allele"

                // Đọc từng dòng dữ liệu
                for (int i = 1; i < lines.Length; i++)
                {
                    var values = ParseCsvLine(lines[i]);

                    if (values.Length > 0 && !string.IsNullOrEmpty(values[0]))
                    {
                        var alleleData = new AlleleData
                        {
                            Allele = values[0].Trim('"')
                        };

                        // Đọc giá trị cho từng marker
                        for (int j = 1; j < Math.Min(values.Length, markers.Length + 1); j++)
                        {
                            var markerName = markers[j - 1];
                            var valueStr = values[j].Trim('"');

                            if (!string.IsNullOrEmpty(valueStr))
                            {
                                var normalizedValue = valueStr.Replace(",", ".");
                                if (double.TryParse(normalizedValue, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedValue))
                                {
                                    alleleData.MarkerValues[markerName] = parsedValue;
                                }
                            }
                            else
                            {
                                alleleData.MarkerValues[markerName] = null;
                            }
                        }

                        alleleList.Add(alleleData);
                    }
                }

                Console.WriteLine($"Đã load {alleleList.Count} records từ file CSV");
                return alleleList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi đọc file CSV: {ex.Message}");
                throw;
            }
        }

        private string[] ParseCsvLine(string line)
        {
            var result = new List<string>();
            var current = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(c);
                }
            }

            result.Add(current.ToString());
            return result.ToArray();
        }

        // Phương thức tìm kiếm theo Allele
        public AlleleData GetByAllele(string allele)
        {
            var data = GetAlleleData();
            return data.FirstOrDefault(x => x.Allele.Equals(allele, StringComparison.OrdinalIgnoreCase));
        }

        // Phương thức tìm kiếm theo Marker
        public List<AlleleData> GetByMarker(string marker)
        {
            var data = GetAlleleData();
            return data.Where(x => x.MarkerValues.ContainsKey(marker) && x.MarkerValues[marker].HasValue)
                      .OrderByDescending(x => x.MarkerValues[marker])
                      .ToList();
        }

        // Thống kê dữ liệu
        public Dictionary<string, object> GetStatistics()
        {
            var data = GetAlleleData();
            var stats = new Dictionary<string, object>();

            stats["TotalAlleles"] = data.Count;
            stats["TotalMarkers"] = data.FirstOrDefault()?.MarkerValues.Count ?? 0;

            // Thống kê theo marker
            var markerStats = new Dictionary<string, object>();
            if (data.Any())
            {
                var firstRecord = data.First();
                foreach (var marker in firstRecord.MarkerValues.Keys)
                {
                    var values = data.Where(x => x.MarkerValues[marker].HasValue)
                                   .Select(x => x.MarkerValues[marker].Value)
                                   .ToList();

                    if (values.Any())
                    {
                        markerStats[marker] = new
                        {
                            Count = values.Count,
                            Min = values.Min(),
                            Max = values.Max(),
                            Average = values.Average()
                        };
                    }
                }
            }

            stats["MarkerStatistics"] = markerStats;
            return stats;
        }
    }
}
