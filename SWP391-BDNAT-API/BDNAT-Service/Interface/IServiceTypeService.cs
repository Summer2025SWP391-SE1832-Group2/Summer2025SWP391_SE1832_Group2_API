using BDNAT_Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Interface
{
    public interface IServiceTypeService
    {
        Task<List<ServiceTypeDTO>> GetAllServiceTypesAsync();
        Task<ServiceTypeDTO> GetServiceTypeByIdAsync(int id);
        Task<bool> CreateServiceTypeAsync(ServiceTypeDTO serviceType);
        Task<bool> UpdateServiceTypeAsync(ServiceTypeDTO serviceType);
        Task<bool> DeleteServiceTypeAsync(int id);
    }
}
