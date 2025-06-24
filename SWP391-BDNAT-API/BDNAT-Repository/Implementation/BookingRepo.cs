using BDNAT_Repository.DTO;
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
                    .Include(b => b.User)
                    .ToListAsync();
            }
        }

        public async Task<List<Booking>> GetBookingByUserIdAsync(int id)
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.Bookings
                    .Include(b => b.SampleCollectionSchedules)
                    .Include(b => b.User)
                    .Include(b => b.Service)
                    .Where(b => b.UserId == id).ToListAsync();
            }
        }

        public async Task<BookingDisplayDetailDTO> GetBookingByIdAsync(int bookingId)
        {
            using (var context = new DnaTestingDbContext())
            {
                var booking = await context.Bookings
                    .Where(b => b.BookingId == bookingId)
                    .Select(b => new BookingDisplayDetailDTO
                    {
                        BookingId = b.BookingId,
                        UserId = b.UserId,
                        BookingDate = b.BookingDate,
                        Status = b.Status,
                        PaymentStatus = b.PaymentStatus,
                        PreferredDate = b.PreferredDate,
                        Method = b.Method,
                        FinalResult = b.FinalResult,
                        FullName = b.User.FullName,

                        // Lấy lịch hẹn cuối cùng (mới nhất)
                        CollectionDate = b.SampleCollectionSchedules
                            .OrderByDescending(s => s.CollectionDate)
                            .ThenByDescending(s => s.Time)
                            .Select(s => s.CollectionDate)
                            .FirstOrDefault(),

                        Time = b.SampleCollectionSchedules
                            .OrderByDescending(s => s.CollectionDate)
                            .ThenByDescending(s => s.Time)
                            .Select(s => s.Time)
                            .FirstOrDefault(),

                        Location = b.SampleCollectionSchedules
                            .OrderByDescending(s => s.CollectionDate)
                            .ThenByDescending(s => s.Time)
                            .Select(s => s.Location)
                            .FirstOrDefault() ?? string.Empty,

                        ResultDetails = b.ResultDetails
                            .Select(r => new ResultDetailDTO
                            {
                                ResultDetailId = r.ResultDetailId,
                                BookingId = r.BookingId,
                                SampleId = r.SampleId,
                                TestParameterId = r.TestParameterId,
                                Value = r.Value,
                                ParameterName = r.TestParameter.Parameter.Name,
                                Description = r.TestParameter.Parameter.Description
                            }).ToList()
                    })
                    .FirstOrDefaultAsync();

                return booking;
            }
        }


        public async Task<List<Booking>> GetAllBookingWithSample()
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.Bookings
                    .Include(b => b.User)
                    .Include(b => b.Samples)
                    .Include(b => b.Service)
                    .ToListAsync();
            }
        }

        public async Task<Booking> GetBookingByOrderCodeAsync(long id)
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.Bookings
                    .FirstOrDefaultAsync(b => b.OrderCode == id);
            }
        }

        public async Task<bool> IsScheduleDuplicatedAsync(DateTime collectionDate, string time, string location)
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.SampleCollectionSchedules
                    .AnyAsync(s =>
                        s.CollectionDate.Date == collectionDate.Date &&
                        s.Time == time &&
                        s.Location == location);
            }
        }

    }
}
