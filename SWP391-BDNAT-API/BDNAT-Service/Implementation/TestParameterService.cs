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
    public class TestParameterService : ITestParameterService
    {
        private readonly IMapper _mapper;

        public TestParameterService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateTestParameterAsync(TestParameterDTO testParameter)
        {
            var map = _mapper.Map<TestParameter>(testParameter);
            return await TestParameterRepo.Instance.InsertAsync(map);
        }

        public async Task<bool> DeleteTestParameterAsync(int id)
        {
            return await TestParameterRepo.Instance.DeleteAsync(id);
        }

        public async Task<List<TestParameterDTO>> GetAllTestParametersAsync()
        {
            var list = await TestParameterRepo.Instance.GetAllAsync();
            return list.Select(x => _mapper.Map<TestParameterDTO>(x)).ToList();
        }

        public async Task<TestParameterDTO> GetTestParameterByIdAsync(int id)
        {
            return _mapper.Map<TestParameterDTO>(await TestParameterRepo.Instance.GetByIdAsync(id));
        }

        public async Task<bool> UpdateTestParameterAsync(TestParameterDTO testParameter)
        {
            var map = _mapper.Map<TestParameter>(testParameter);
            return await TestParameterRepo.Instance.UpdateAsync(map);
        }
    }

}
