using BDNAT_Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.Interface
{
    public interface IBookingRepo : IGenericRepository<Booking>
    {
        Task<List<Booking>> GetAllBookingWithSchedule();
        Task<Booking> GetBookingByUserIdAsync(int id);
    }
}
