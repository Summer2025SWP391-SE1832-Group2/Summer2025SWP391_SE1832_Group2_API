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
    public class SampleService : ISampleService
    {
        private readonly IMapper _mapper;

        public SampleService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateSampleAsync(SampleDTO sample)
        {
            var map = _mapper.Map<Sample>(sample);
            return await SampleRepo.Instance.InsertAsync(map);
        }

        public async Task<bool> DeleteSampleAsync(int id)
        {
            return await SampleRepo.Instance.DeleteAsync(id);
        }

        public async Task<List<SampleDTO>> GetAllSamplesAsync()
        {
            var list = await SampleRepo.Instance.GetAllAsync();
            return list.Select(x => _mapper.Map<SampleDTO>(x)).ToList();
        }

        public async Task<List<SampleWithCollectorDTO>> GetSampleByBookingIdAsync(int bookingId)
        {
            var samples = await SampleRepo.Instance.GetSamplesByBookingIdAsync(bookingId);
            var updateBooking = await BookingRepo.Instance.GetById(bookingId);
            updateBooking.Status = "Đã lấy mẫu";
            var check = await BookingRepo.Instance.UpdateAsync(updateBooking);
            var result = samples.Select(s => new SampleWithCollectorDTO
            {
                SampleId = s.SampleId,
                BookingId = s.BookingId,
                CollectedBy = s.CollectedBy?.ToString(),
                CollectorName = s.CollectedByNavigation?.FullName,
                CollectedDate = s.CollectedDate,
                SampleType = s.SampleType,
                ParticipantName = s.ParticipantName,
                Notes = s.Notes,
                Picture = s.Picture,
                Transport = s.Transport
            }).ToList();

            return result;
        }

        public async Task<SampleDTO> GetSampleByIdAsync(int id)
        {
            return _mapper.Map<SampleDTO>(await SampleRepo.Instance.GetByIdAsync(id));
        }

        public async Task<bool> UpdateSampleAsync(SampleDTO sample)
        {
            var map = _mapper.Map<Sample>(sample);
            return await SampleRepo.Instance.UpdateAsync(map);
        }
    }

}
