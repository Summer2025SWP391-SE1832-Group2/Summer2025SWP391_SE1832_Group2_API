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
    public class TestKitService : ITestKitService
    {
        private readonly IMapper _mapper;

        public TestKitService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateTestKitAsync(TestKitDTO testKit)
        {
            var map = _mapper.Map<TestKit>(testKit);
            return await TestKitRepo.Instance.InsertAsync(map);
        }

        public async Task<bool> DeleteTestKitAsync(int id)
        {
            return await TestKitRepo.Instance.DeleteAsync(id);
        }

        public async Task<List<TestKitDTO>> GetAllTestKitsAsync()
        {
            var list = await TestKitRepo.Instance.GetAllAsync();
            return list.Select(x => _mapper.Map<TestKitDTO>(x)).ToList();
        }

        public async Task<TestKitDTO> GetTestKitByIdAsync(int id)
        {
            return _mapper.Map<TestKitDTO>(await TestKitRepo.Instance.GetByIdAsync(id));
        }

        public async Task<bool> UpdateTestKitAsync(TestKitDTO testKit)
        {
            var map = _mapper.Map<TestKit>(testKit);
            return await TestKitRepo.Instance.UpdateAsync(map);
        }
    }

}
