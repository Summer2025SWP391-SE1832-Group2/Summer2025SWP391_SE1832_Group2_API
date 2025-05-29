using BDNAT_Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Interface
{
    public interface IServiceService
    {
        Task<List<ServiceDTO>> GetAllServicesAsync();
        Task<ServiceDTO> GetServiceByIdAsync(int id);
        Task<bool> CreateServiceAsync(ServiceDTO service);
        Task<bool> UpdateServiceAsync(ServiceDTO service);
        Task<bool> DeleteServiceAsync(int id);
    }
}
