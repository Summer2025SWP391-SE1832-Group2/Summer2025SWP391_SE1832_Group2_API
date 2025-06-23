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

        public async Task<List<TestParameterResultDTO>> GetResultWithParameterInfoAsync(int bookingId)
        {
            var booking = await BookingRepo.Instance.GetByIdAsync(bookingId);
            if (booking == null) return null;
            int serviceId = booking.ServiceId;
            var resultDetails = await ResultDetailRepo.Instance.GetResultDetailsByBookingIdAsync(bookingId);
            var testParameters = await TestParameterRepo.Instance.GetTestParametersByServiceIdAsync(serviceId);

            var result = (from tp in testParameters
                          join rd in resultDetails on tp.TestParameterId equals rd.TestParameterId into gj
                          from subResult in gj.DefaultIfEmpty()
                          where subResult != null // CHỈ lấy những dòng có kết quả thực sự
                          select new TestParameterResultDTO
                          {
                              ResultDetailId = subResult.ResultDetailId,
                              TestParameterId = tp.TestParameterId,
                              Name = tp.Parameter?.Name,
                              Unit = tp.Parameter?.Unit,
                              Description = tp.Parameter?.Description,
                              Value = subResult.Value,
                              SampleId = subResult.SampleId
                          }).ToList();


            return result;
        }

        public async Task<List<TestParameterFormDTO>> GetTestParameterFormAsync(int bookingId)
        {
            var booking = await BookingRepo.Instance.GetByIdAsync(bookingId);
            if (booking == null) return null;
            int serviceId = booking.ServiceId;
            var resultDetails = await ResultDetailRepo.Instance.GetResultDetailsByBookingIdAsync(bookingId);
            var testParameters = await TestParameterRepo.Instance.GetTestParametersByServiceIdAsync(serviceId);

            var result = testParameters.Select(tp => new TestParameterFormDTO
            {
                TestParameterId = tp.TestParameterId,
                Name = tp.Parameter?.Name,
                Unit = tp.Parameter?.Unit,
                Description = tp.Parameter?.Description
            }).ToList();

            return result;
        }


        public async Task<bool> UpdateTestParameterAsync(TestParameterDTO testParameter)
        {
            var map = _mapper.Map<TestParameter>(testParameter);
            return await TestParameterRepo.Instance.UpdateAsync(map);
        }
    }

}
