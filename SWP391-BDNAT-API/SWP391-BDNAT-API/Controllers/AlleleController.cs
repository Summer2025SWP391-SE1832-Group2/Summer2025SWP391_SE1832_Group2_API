using BDNAT_Service.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace SWP391_BDNAT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlleleController : ControllerBase
    {
        private readonly AlleleDataService _alleleDataService;

        public AlleleController(AlleleDataService alleleDataService)
        {
            _alleleDataService = alleleDataService;
        }

        [HttpGet]
        public IActionResult GetAllAlleles()
        {
            try
            {
                var data = _alleleDataService.GetAlleleData();
                return Ok(new { success = true, data = data, count = data.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpGet("{allele}")]
        public IActionResult GetByAllele(string allele)
        {
            try
            {
                var data = _alleleDataService.GetByAllele(allele);
                if (data == null)
                {
                    return NotFound(new { success = false, message = $"Không tìm thấy allele: {allele}" });
                }

                return Ok(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpGet("marker/{marker}")]
        public IActionResult GetByMarker(string marker)
        {
            try
            {
                var data = _alleleDataService.GetByMarker(marker);
                return Ok(new { success = true, data = data, count = data.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpGet("statistics")]
        public IActionResult GetStatistics()
        {
            try
            {
                var stats = _alleleDataService.GetStatistics();
                return Ok(new { success = true, statistics = stats });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpGet("search")]
        public IActionResult SearchAlleles([FromQuery] string query, [FromQuery] string marker = null)
        {
            try
            {
                var allData = _alleleDataService.GetAlleleData();
                var filteredData = allData.AsQueryable();

                if (!string.IsNullOrEmpty(query))
                {
                    filteredData = filteredData.Where(x => x.Allele.Contains(query, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrEmpty(marker))
                {
                    filteredData = filteredData.Where(x => x.MarkerValues.ContainsKey(marker) && x.MarkerValues[marker].HasValue);
                }

                var result = filteredData.ToList();
                return Ok(new { success = true, data = result, count = result.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpGet("export")]
        public IActionResult ExportToJson()
        {
            try
            {
                var data = _alleleDataService.GetAlleleData();
                var jsonData = System.Text.Json.JsonSerializer.Serialize(data, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var bytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
                return File(bytes, "application/json", "allele_data.json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}
