using AutoMapper;
using BDNAT_Repository.Entities;
using BDNAT_Repository.Implementation;
using BDNAT_Repository.DTO;
using BDNAT_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Implementation
{
    public class KitOrderService : IKitOrderService
    {
        private readonly IMapper _mapper;

        public KitOrderService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateKitOrderAsync(KitOrderDTO kitOrder)
        {
            var map = _mapper.Map<KitOrder>(kitOrder);
            return await KitOrderRepo.Instance.InsertAsync(map);
        }

        public async Task<bool> DeleteKitOrderAsync(int id)
        {
            return await KitOrderRepo.Instance.DeleteAsync(id);
        }

        public async Task<List<KitOrderDTO>> GetAllKitOrdersAsync()
        {
            var list = await KitOrderRepo.Instance.GetAllAsync();
            return list.Select(x => _mapper.Map<KitOrderDTO>(x)).ToList();
        }

        public async Task<KitOrderDTO> GetKitOrderByIdAsync(int id)
        {
            return _mapper.Map<KitOrderDTO>(await KitOrderRepo.Instance.GetByIdAsync(id));
        }

        public async Task<bool> UpdateKitOrderAsync(KitOrderDTO kitOrder)
        {
            var map = _mapper.Map<KitOrder>(kitOrder);
            return await KitOrderRepo.Instance.UpdateAsync(map);
        }
    }

}
