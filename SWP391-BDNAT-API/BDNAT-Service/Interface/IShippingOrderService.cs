using BDNAT_Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Interface
{
    public interface IShippingOrderService
    {
        Task<List<ShippingOrderDTO>> GetAllShippingOrdersAsync();
        Task<ShippingOrderDTO> GetShippingOrderByIdAsync(int id);
        Task<bool> CreateShippingOrderAsync(ShippingOrderDTO shippingOrder);
        Task<bool> UpdateShippingOrderAsync(ShippingOrderDTO shippingOrder);
        Task<bool> DeleteShippingOrderAsync(int id);
        Task<List<ShippingOrderDTO>> GetShippingOrderByBookingIdAsync(int bookingId);

    }
}
