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
    public class SampleRepo : GenericRepository<Sample>, ISampleRepo
    {
        private static SampleRepo _instance;

        public static SampleRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SampleRepo();
                }
                return _instance;
            }
        }


        public async Task<List<Sample>> GetSamplesByBookingIdAsync(int bookingId)
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.Samples
                    .Include(s => s.CollectedByNavigation) // assuming navigation property is named properly
                    .Where(s => s.BookingId == bookingId)
                    .ToListAsync();
            }
        }

    }
}
