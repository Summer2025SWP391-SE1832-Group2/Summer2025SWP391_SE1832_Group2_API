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
        Task<List<BookingDTO>> GetAllBookingsAsync();
        Task<List<BookingDTO>> GetAllBookingWithScheduleAsync();

        Task<BookingDTO> GetBookingByIdAsync(int id);
        Task<string> CreateBookingAsync(BookingDTO booking);
        Task<bool> UpdateBookingAsync(BookingDTO booking);
        Task<bool> DeleteBookingAsync(int id);
    }
}
