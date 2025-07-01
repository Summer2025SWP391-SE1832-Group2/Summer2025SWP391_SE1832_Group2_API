using BDNAT_Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Interface
{
    public interface IBookingService
    {
        Task<List<BookingDisplayDTO>> GetAllBookingsAsync();
        Task<bool> CheckPending(int uid);
        Task<List<BookingScheduleDTO>> GetAllBookingWithScheduleAsync();
        Task<List<BookingSampleDTO>> GetAllBookingWithSampleAsync();
        Task<BookingDisplayDetailDTO> GetBookingByIdAsync(int id);
        Task<List<BookingDisplayDTO>> GetBookingByUserIdAsync(int id);
        Task<string?> CreateBookingAsync(BookingRequestDTO booking);
        Task<bool> UpdateBookingAsync(BookingRequestDTO booking);
        Task<bool> DeleteBookingAsync(int id);
        Task<string?> RegeneratePaymentQrAsync(int bookingId);
        Task<List<BookingScheduleDTO>> GetBookingsByCollectorAsync(int collectorId);
    }
}
