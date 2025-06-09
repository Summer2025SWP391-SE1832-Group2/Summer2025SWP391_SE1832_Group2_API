using BDNAT_Repository.Entities;
using BDNAT_Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.Implementation
{
    public class BookingRepo : GenericRepository<Booking>, IBookingRepo
    {
        private static BookingRepo _instance;

        public static BookingRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BookingRepo();
                }
                return _instance;
            }
        }


        public async Task<List<Booking>> GetAllBookingWithSchedule()
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.Bookings
                    .Include(b => b.SampleCollectionSchedules) 
                    .ToListAsync();
            }
        }

        public async Task<List<Booking>> GetBookingByUserIdAsync(int id)
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.Bookings
                    .Include(b => b.SampleCollectionSchedules)
                    .Where(b => b.UserId == id).ToListAsync();
            }
        }

    }
}
