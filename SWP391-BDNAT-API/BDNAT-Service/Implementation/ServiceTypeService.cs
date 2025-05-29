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
    public class ServiceTypeService : IServiceTypeService
    {
        private readonly IMapper _mapper;

        public ServiceTypeService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateServiceTypeAsync(ServiceTypeDTO serviceType)
        {
            var map = _mapper.Map<ServiceType>(serviceType);
            return await ServiceTypeRepo.Instance.InsertAsync(map);
        }

        public async Task<bool> DeleteServiceTypeAsync(int id)
        {
            return await ServiceTypeRepo.Instance.DeleteAsync(id);
        }

        public async Task<List<ServiceTypeDTO>> GetAllServiceTypesAsync()
        {
            var list = await ServiceTypeRepo.Instance.GetAllAsync();
            return list.Select(x => _mapper.Map<ServiceTypeDTO>(x)).ToList();
        }

        public async Task<ServiceTypeDTO> GetServiceTypeByIdAsync(int id)
        {
            return _mapper.Map<ServiceTypeDTO>(await ServiceTypeRepo.Instance.GetByIdAsync(id));
        }

        public async Task<bool> UpdateServiceTypeAsync(ServiceTypeDTO serviceType)
        {
            var map = _mapper.Map<ServiceType>(serviceType);
            return await ServiceTypeRepo.Instance.UpdateAsync(map);
        }
    }

}
