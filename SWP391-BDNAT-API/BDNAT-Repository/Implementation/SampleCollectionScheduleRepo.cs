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
    public class SampleCollectionScheduleRepo : GenericRepository<SampleCollectionSchedule>, ISampleCollectionScheduleRepo
    {
        private static SampleCollectionScheduleRepo _instance;

        public static SampleCollectionScheduleRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SampleCollectionScheduleRepo();
                }
                return _instance;
            }
        }

        public async Task<SampleCollectionSchedule> GetScheduleByBookingIdAsync(int bookingId)
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.SampleCollectionSchedules
                    .Include(s => s.Booking)
                    .Include(s => s.Collector)
                    .FirstOrDefaultAsync(s => s.BookingId == bookingId);
            }
        }

        public async Task<List<SampleCollectionSchedule>> GetSchedulesByCollectorIdAsync(int collectorId)
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.SampleCollectionSchedules
                    .Include(s => s.Booking)
                    .Where(s => s.CollectorId == collectorId)
                    .ToListAsync();
            }
        }
    }
} 