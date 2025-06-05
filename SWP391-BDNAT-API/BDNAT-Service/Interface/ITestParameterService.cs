using BDNAT_Repository.DTO;
using BDNAT_Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Interface
{
    public interface ITestParameterService
    {
        Task<List<TestParameterDTO>> GetAllTestParametersAsync();
        Task<TestParameterDTO> GetTestParameterByIdAsync(int id);
        Task<bool> CreateTestParameterAsync(TestParameterDTO testParameter);
        Task<bool> UpdateTestParameterAsync(TestParameterDTO testParameter);
        Task<bool> DeleteTestParameterAsync(int id);
        Task<List<TestParameterAndValueDTO>> GetTestParametersByServiceIdAsync(int ServiceId);
    }
}
