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
    public class ServiceService : IServiceService
    {
        private readonly IMapper _mapper;

        public ServiceService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateServiceAsync(ServiceDTO service)
        {
            var map = _mapper.Map<Service>(service);
            return await ServiceRepo.Instance.InsertAsync(map);
        }

        public async Task<bool> DeleteServiceAsync(int id)
        {
            return await ServiceRepo.Instance.DeleteAsync(id);
        }

        public async Task<List<ServiceDTO>> GetAllServicesAsync()
        {
            var list = await ServiceRepo.Instance.GetAllAsync();
            return list.Select(x => _mapper.Map<ServiceDTO>(x)).ToList();
        }

        public async Task<ServiceDTO> GetServiceByIdAsync(int id)
        {
            return _mapper.Map<ServiceDTO>(await ServiceRepo.Instance.GetByIdAsync(id));
        }

        public async Task<bool> UpdateServiceAsync(ServiceDTO service)
        {
            var map = _mapper.Map<Service>(service);
            return await ServiceRepo.Instance.UpdateAsync(map);
        }
    }

}
