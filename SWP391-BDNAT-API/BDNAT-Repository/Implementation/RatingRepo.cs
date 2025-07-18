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
    public class RatingRepo : GenericRepository<Rating>, IRatingRepo
    {
        private static RatingRepo _instance;

        public static RatingRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RatingRepo();
                }
                return _instance;
            }
        }

        public async Task<List<Rating>> GetAllRatingByBookingIdAsync(int bookId)
        {
            var comments = await _context.Ratings
                .Where(c => c.BookingId == bookId)
                .ToListAsync();

            return comments;
        }
    }
}
