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

        public async Task<bool> UpdateSamplePictureAndBookingStatusAsync(SamplePictureUpdateDTO dto)
        {
            // 1. Lấy sample theo ID
            var sample = await SampleRepo.Instance.GetByIdAsync(dto.SampleId);
            if (sample == null)
                return false;

            // 2. Cập nhật ảnh
            sample.Picture = dto.Picture;

            // 3. Cập nhật sample
            var sampleUpdated = await SampleRepo.Instance.UpdateAsync(sample);
            if (!sampleUpdated)
                return false;

            // 4. Kiểm tra số lượng sample đã thu thập cho booking tương ứng
            if (sample.BookingId.HasValue)
            {
                var bookingId = sample.BookingId.Value;

                var allSamples = await SampleRepo.Instance.GetAllAsync();
                var collectedSamples = allSamples
                    .Where(s => s.BookingId == bookingId && !string.IsNullOrEmpty(s.Picture))
                    .ToList();

                var booking = await BookingRepo.Instance.GetByIdAsync(bookingId);
                if (booking == null)
                    return false;

                // 5. Cập nhật trạng thái booking theo số lượng mẫu
                if (collectedSamples.Count >= 2)
                    booking.Status = "Đã lấy mẫu";
                else if (collectedSamples.Count == 1)
                    booking.Status = "Đang lấy mẫu";
                else
                    booking.Status = "Chờ lấy mẫu";

                return await BookingRepo.Instance.UpdateAsync(booking);
            }

            return true; // Sample có thể không liên kết Booking
        }


    }

}
