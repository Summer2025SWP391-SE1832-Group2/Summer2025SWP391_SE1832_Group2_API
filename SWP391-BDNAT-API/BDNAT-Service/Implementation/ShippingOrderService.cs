using AutoMapper;
using BDNAT_Repository.Entities;
using BDNAT_Repository.Implementation;
using BDNAT_Service.DTO;
using BDNAT_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Implementation
{
    public class ShippingOrderService : IShippingOrderService
    {
        private readonly IMapper _mapper;

        public ShippingOrderService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateShippingOrderAsync(ShippingOrderDTO shippingOrder)
        {
            var map = _mapper.Map<ShippingOrder>(shippingOrder);
            return await ShippingOrderRepo.Instance.InsertAsync(map);
        }

        public async Task<bool> DeleteShippingOrderAsync(int id)
        {
            return await ShippingOrderRepo.Instance.DeleteAsync(id);
        }

        public async Task<List<ShippingOrderDTO>> GetAllShippingOrdersAsync()
        {
            var list = await ShippingOrderRepo.Instance.GetAllAsync();
            return list.Select(x => _mapper.Map<ShippingOrderDTO>(x)).ToList();
        }

        public async Task<ShippingOrderDTO> GetShippingOrderByIdAsync(int id)
        {
            return _mapper.Map<ShippingOrderDTO>(await ShippingOrderRepo.Instance.GetByIdAsync(id));
        }

        public async Task<bool> UpdateShippingOrderAsync(ShippingOrderDTO shippingOrder)
        {
            var map = _mapper.Map<ShippingOrder>(shippingOrder);
            return await ShippingOrderRepo.Instance.UpdateAsync(map);
        }
    }

}
