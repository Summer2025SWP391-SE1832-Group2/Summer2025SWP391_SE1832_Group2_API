using BDNAT_Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Interface
{
    public interface IParameterService
    {
        Task<List<ParameterDTO>> GetAllParametersAsync();
        Task<ParameterDTO> GetParameterByIdAsync(int id);
        Task<bool> CreateParameterAsync(ParameterDTO parameter);
        Task<bool> UpdateParameterAsync(ParameterDTO parameter);
        Task<bool> DeleteParameterAsync(int id);
    }
}
