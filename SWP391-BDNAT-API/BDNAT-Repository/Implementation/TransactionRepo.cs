using BDNAT_Repository.DTO;
using BDNAT_Repository.Entities;
using BDNAT_Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.Implementation
{
    public class TransactionRepo : GenericRepository<Transaction>, ITransactionRepo
    {
        private static TransactionRepo _instance;

        public static TransactionRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TransactionRepo();
                }
                return _instance;
            }
        }

        public async Task<Transaction> GetByOrderCodeAsync(long orderCode)
        {
            return await _context.Transactions.FirstOrDefaultAsync(t => t.OrderCode == orderCode);
        }

        public async Task<List<Transaction>> GetTransactionByUserIdAsync(int id)
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.Transactions
                    .Where(b => b.UserId == id).ToListAsync();
            }
        }

        public async Task<TotalTransactionAmountsDto> GetTotalAmountsAsync()
        {
            var transactions = await _context.Transactions.ToListAsync();

            var dto = new TotalTransactionAmountsDto
            {
                TotalPaidAmount = transactions
                    .Where(t => t.Status == "Đã thanh toán")
                    .Sum(t => t.Price ?? 0),

                TotalRefundedAmount = transactions
                    .Where(t => t.Status == "Đã hoàn tiền")
                    .Sum(t => t.Price ?? 0),

                TotalSuccessfulTransactions = transactions
                    .Count(t => t.Status == "Đã thanh toán"),

                TotalRefundedTransactions = transactions
                    .Count(t => t.Status == "Đã hoàn tiền")
            };

            return dto;
        }

        public async Task<List<RevenuePeriodDto>> GetTransactionStatsAsync(int year, int? month = null, int? week = null)
        {
            var query = _context.Transactions
                .Where(t => t.Status == "Đã thanh toán" && t.CreatedAt.Year == year);

            if (month.HasValue)
                query = query.Where(t => t.CreatedAt.Month == month.Value);

            var result = new List<RevenuePeriodDto>();

            if (week.HasValue && !month.HasValue) // Trường hợp tuần theo năm
            {
                var firstDayOfYear = new DateTime(year, 1, 1);
                var cal = CultureInfo.InvariantCulture.Calendar;
                var startOfWeek = Enumerable.Range(0, 366)
                    .Select(offset => firstDayOfYear.AddDays(offset))
                    .FirstOrDefault(d => cal.GetWeekOfYear(d, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) == week.Value && d.DayOfWeek == DayOfWeek.Monday);

                if (startOfWeek == default)
                    return result;

                for (int i = 0; i < 7; i++)
                {
                    var day = startOfWeek.AddDays(i);
                    var total = await query.Where(t => t.CreatedAt.Date == day.Date).SumAsync(t => t.Price ?? 0);
                    result.Add(new RevenuePeriodDto
                    {
                        Label = day.ToString("dd/MM"),
                        Total = total
                    });
                }
            }
            else if (month.HasValue) // Trường hợp theo tuần trong tháng
            {
                var daysInMonth = DateTime.DaysInMonth(year, month.Value);
                for (int weekNum = 0; weekNum < 4; weekNum++)
                {
                    var startDay = (weekNum * 7) + 1;
                    var endDay = Math.Min(startDay + 6, daysInMonth);

                    var startDate = new DateTime(year, month.Value, startDay);
                    var endDate = new DateTime(year, month.Value, endDay);

                    var total = await query.Where(t => t.CreatedAt.Date >= startDate && t.CreatedAt.Date <= endDate).SumAsync(t => t.Price ?? 0);
                    result.Add(new RevenuePeriodDto
                    {
                        Label = $"Tuần {weekNum + 1} ({startDay}-{endDay})",
                        Total = total
                    });
                }
            }
            else // Theo tháng
            {
                for (int m = 1; m <= 12; m++)
                {
                    var total = await query.Where(t => t.CreatedAt.Month == m).SumAsync(t => t.Price ?? 0);
                    result.Add(new RevenuePeriodDto
                    {
                        Label = $"Tháng {m}",
                        Total = total
                    });
                }
            }

            return result;
        }
    }
}
