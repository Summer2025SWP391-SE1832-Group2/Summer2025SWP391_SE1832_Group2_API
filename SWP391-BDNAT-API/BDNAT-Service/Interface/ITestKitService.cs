using BDNAT_Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Interface
{
    public interface ITestKitService
    {
        Task<List<TestKitDTO>> GetAllTestKitsAsync();
        Task<TestKitDTO> GetTestKitByIdAsync(int id);
        Task<bool> CreateTestKitAsync(TestKitDTO testKit);
        Task<bool> UpdateTestKitAsync(TestKitDTO testKit);
        Task<bool> DeleteTestKitAsync(int id);
    }
}
