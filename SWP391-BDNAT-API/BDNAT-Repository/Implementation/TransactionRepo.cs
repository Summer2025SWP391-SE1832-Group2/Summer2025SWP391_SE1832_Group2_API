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
    }
}
