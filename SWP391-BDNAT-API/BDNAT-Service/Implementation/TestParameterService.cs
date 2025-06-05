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
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<TestParameterAndValueDTO>> GetTestParametersByServiceIdAsync(int serviceId)
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.TestParameters
                    .Include(tp => tp.Parameter)
                    .Where(tp => tp.ServiceId == serviceId)
                    .Select(tp => new TestParameterAndValueDTO
                    {
                        TestParameterId = tp.TestParameterId,
                        ServiceId = tp.ServiceId,
                        ParameterId = tp.ParameterId,
                        DisplayOrder = tp.DisplayOrder,
                        Name = tp.Parameter != null ? tp.Parameter.Name : null,
                        Unit = tp.Parameter != null ? tp.Parameter.Unit : null,
                        Description = tp.Parameter != null ? tp.Parameter.Description : null
                    })
                    .ToListAsync();
            }
        }

        public async Task<bool> UpdateTestParameterAsync(TestParameterDTO testParameter)
        {
            var map = _mapper.Map<TestParameter>(testParameter);
            return await TestParameterRepo.Instance.UpdateAsync(map);
        }
    }

}
