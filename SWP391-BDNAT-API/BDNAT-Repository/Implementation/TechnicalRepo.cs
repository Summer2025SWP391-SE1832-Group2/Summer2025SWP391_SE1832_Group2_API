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
    public class TechnicalRepo : GenericRepository<Technical>, ITechnicalRepo
    {
        private static TechnicalRepo _instance;

        public static TechnicalRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TechnicalRepo();
                }
                return _instance;
            }
        }

        public async Task<List<Technical>> GetTechnicalsWithServicesAsync()
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.Technicals
                    .Include(t => t.TechnicalServices)
                        .ThenInclude(ts => ts.Service)
                    .ToListAsync();
            }
        }

        public async Task<Technical> GetTechnicalWithServicesByIdAsync(int technicalId)
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.Technicals
                    .Include(t => t.TechnicalServices)
                        .ThenInclude(ts => ts.Service)
                    .FirstOrDefaultAsync(t => t.TechnicalId == technicalId);
            }
        }
    }
} 