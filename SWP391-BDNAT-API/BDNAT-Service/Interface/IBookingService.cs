using BDNAT_Service.DTO;
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
        Task<BookingDTO> GetBookingByIdAsync(int id);
        Task<bool> CreateBookingAsync(BookingDTO booking);
        Task<bool> UpdateBookingAsync(BookingDTO booking);
        Task<bool> DeleteBookingAsync(int id);
    }
}
