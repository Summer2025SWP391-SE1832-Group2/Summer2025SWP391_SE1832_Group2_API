using BDNAT_Repository.Entities;
using BDNAT_Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.Implementation
{
    public class ResultDetailRepo : GenericRepository<ResultDetail>, IResultDetailRepo
    {
        private static ResultDetailRepo _instance;

        public static ResultDetailRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ResultDetailRepo();
                }
                return _instance;
            }
        }

        public async Task<List<ResultDetail>> GetResultDetailsByBookingIdAsync(int BookingId)
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.ResultDetails
                    .Where(s => s.BookingId == BookingId)
                    .ToListAsync();
            }
        }

        public async Task AddRangeAsync(IEnumerable<ResultDetail> resultDetails)
        {
            try
            {
                _context.ResultDetails.AddRange(resultDetails);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                _context.ChangeTracker.Clear();
                throw;
            }
        }

        public async Task<bool> DeleteWhereAsync(Expression<Func<ResultDetail, bool>> predicate)
        {
            var resultDetails = await _context.ResultDetails
                .Where(predicate)
                .ToListAsync();

            if (!resultDetails.Any())
                return false;

            _context.ResultDetails.RemoveRange(resultDetails);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
