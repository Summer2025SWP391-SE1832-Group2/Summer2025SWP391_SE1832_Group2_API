using BDNAT_Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Interface
{
    public interface IKitOrderService
    {
        Task<List<KitOrderDTO>> GetAllKitOrdersAsync();
        Task<KitOrderDTO> GetKitOrderByIdAsync(int id);
        Task<bool> CreateKitOrderAsync(KitOrderDTO kitOrder);
        Task<bool> UpdateKitOrderAsync(KitOrderDTO kitOrder);
        Task<bool> DeleteKitOrderAsync(int id);
    }
}
