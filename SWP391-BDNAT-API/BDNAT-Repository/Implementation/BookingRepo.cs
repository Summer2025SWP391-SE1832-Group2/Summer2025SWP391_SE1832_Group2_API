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

        public async Task<List<BookingDisplayDTO>> GetAllBookingDisplaysAsync()
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.Bookings
                    .Select(b => new BookingDisplayDTO
                    {
                        BookingId = b.BookingId,
                        UserId = b.UserId,
                        ServiceId = b.ServiceId,
                        ServiceName = b.Service.Name,
                        BookingDate = b.BookingDate,
                        Status = b.Status,
                        PaymentStatus = b.PaymentStatus,
                        PreferredDate = b.PreferredDate,
                        Method = b.Method,
                        OrderCode = b.OrderCode,
                        CollectionDate = b.SampleCollectionSchedules
                                            .OrderBy(s => s.CollectionDate)
                                            .Select(s => s.CollectionDate)
                                            .FirstOrDefault(),
                        Time = b.SampleCollectionSchedules
                                    .OrderBy(s => s.CollectionDate)
                                    .Select(s => s.Time)
                                    .FirstOrDefault(),
                        Location = b.SampleCollectionSchedules
                                        .OrderBy(s => s.CollectionDate)
                                        .Select(s => s.Location)
                                        .FirstOrDefault() ?? "",
                        hasSubmittedRating = b.Ratings.Any()
                    })
                    .ToListAsync();
            }
        }


        public async Task<List<BookingScheduleDTO>> GetAllBookingSchedulesAsync()
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.Bookings
                    .Select(b => new BookingScheduleDTO
                    {
                        BookingId = b.BookingId,
                        UserId = b.UserId,
                        FullName = b.User.FullName,
                        BookingDate = b.BookingDate,
                        Status = b.Status,
                        PaymentStatus = b.PaymentStatus,
                        PreferredDate = b.PreferredDate,
                        Method = b.Method,
                        OrderCode = b.OrderCode,
                        SampleCollectionSchedules = b.SampleCollectionSchedules.Select(s => new SampleCollectionScheduleDTO
                        {
                            ScheduleId = s.ScheduleId,
                            BookingId = s.BookingId,
                            CollectionDate = s.CollectionDate,
                            Time = s.Time,
                            Location = s.Location,
                            CollectorId = s.CollectorId,
                            CollectorName = s.Collector.FullName
                        }).ToList()
                    })
                    .ToListAsync();
            }
        }

        public async Task<List<BookingDisplayDTO>> GetBookingByUserIdAsync(int id)
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.Bookings
                .Where(b => b.UserId == id)
                .Select(b => new BookingDisplayDTO
                {
                    BookingId = b.BookingId,
                    UserId = b.UserId,
                    ServiceId = b.ServiceId,
                    ServiceName = b.Service.Name,
                    BookingDate = b.BookingDate,
                    Status = b.Status,
                    PaymentStatus = b.PaymentStatus,
                    PreferredDate = b.PreferredDate,
                    Method = b.Method,
                    OrderCode = b.OrderCode,
                    CollectionDate = b.SampleCollectionSchedules
                .OrderBy(s => s.CollectionDate)
                .Select(s => s.CollectionDate)
                .FirstOrDefault(),
                    Time = b.SampleCollectionSchedules
                .OrderBy(s => s.CollectionDate)
                .Select(s => s.Time)
                .FirstOrDefault(),
                    Location = b.SampleCollectionSchedules
                .OrderBy(s => s.CollectionDate)
                .Select(s => s.Location)
                .FirstOrDefault() ?? "",
                    hasSubmittedRating = b.Ratings.Any()
                })
                .ToListAsync();
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
                        ServiceId = b.ServiceId,
                        BookingDate = b.BookingDate,
                        Status = b.Status,
                        PaymentStatus = b.PaymentStatus,
                        PreferredDate = b.PreferredDate,
                        Method = b.Method,
                        FinalResult = b.FinalResult,
                        FullName = b.User.FullName,
                        OrderCode = b.OrderCode,
                        ServiceName = b.Service.Name,

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

                        hasSubmittedRating = b.Ratings.Any(),

                        ResultDetails = b.ResultDetails
                            .Select(r => new ResultDetailDTO
                            {
                                ResultDetailId = r.ResultDetailId,
                                BookingId = r.BookingId,
                                SampleId = r.SampleId,
                                TestParameterId = r.TestParameterId,
                                Value = r.Value,
                                ParameterName = r.TestParameter.Parameter.Name,
                                Description = r.TestParameter.Parameter.Description,
                                SampleOwnerName = r.Sample.ParticipantName,
                                Pi = r.Pi
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

        public async Task<bool> IsScheduleDuplicatedAsync(DateTime collectionDate, string time, string location, int userId)
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.SampleCollectionSchedules
                    .AnyAsync(s =>
                        s.CollectionDate.Date == collectionDate.Date &&
                        s.Time == time &&
                        s.Location == location &&
                        s.Booking.UserId == userId // chỉ check lịch của chính user
                    );
            }
        }

        public async Task<List<Booking>> GetBookingsByCollectorIdAsync(int collectorId)
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.Bookings
                    .Include(b => b.User)
                    .Include(b => b.SampleCollectionSchedules.Where(s => s.CollectorId == collectorId))
                        .ThenInclude(s => s.Collector)
                    .Where(b => b.SampleCollectionSchedules.Any(s => s.CollectorId == collectorId))
                    .ToListAsync();
            }
        }

        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Transactions)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);

            if (booking == null || booking.Status == "Đã hủy")
                return false;

            // Cập nhật trạng thái booking
            booking.Status = "Đã hủy";
            booking.PaymentStatus = "Đang chờ xử lý";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RefundBookingAsync(long orderCode)
        {
            var booking = await _context.Bookings
                .Include(b => b.Transactions)
                .FirstOrDefaultAsync(b => b.OrderCode == orderCode);
            if(booking == null || booking.Status != "Đã hủy")
            {
                return false;
            }
            booking.PaymentStatus = "Đã Hoàn tiền";

            // Cập nhật transaction (nếu có)
            foreach (var trans in booking.Transactions)
            {
                trans.Status = "Đã hoàn tiền";
                trans.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
