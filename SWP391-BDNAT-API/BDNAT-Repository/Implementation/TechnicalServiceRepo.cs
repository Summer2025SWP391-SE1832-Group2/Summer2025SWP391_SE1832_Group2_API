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
    public class TechnicalServiceRepo : GenericRepository<TechnicalService>, ITechnicalServiceRepo
    {
        private static TechnicalServiceRepo _instance;

        public static TechnicalServiceRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TechnicalServiceRepo();
                }
                return _instance;
            }
        }

        public async Task<List<TechnicalService>> GetTechnicalServicesByTechnicalIdAsync(int technicalId)
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.TechnicalServices
                    .Include(ts => ts.Service)
                    .Where(ts => ts.TechnicalId == technicalId)
                    .ToListAsync();
            }
        }

        public async Task<List<TechnicalService>> GetTechnicalServicesByServiceIdAsync(int serviceId)
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.TechnicalServices
                    .Include(ts => ts.Technical)
                    .Where(ts => ts.ServiceId == serviceId)
                    .ToListAsync();
            }
        }
    }
} 