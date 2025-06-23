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
    public class TestParameterRepo : GenericRepository<TestParameter>, ITestParameterRepo
    {
        private static TestParameterRepo _instance;

        public static TestParameterRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TestParameterRepo();
                }
                return _instance;
            }
        }

        public async Task<List<TestParameter>> GetTestParametersByServiceIdAsync(int serviceId)
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.TestParameters
                    .Include(tp => tp.Parameter)
                    .Where(tp => tp.ServiceId == serviceId)
                    .ToListAsync();
            }
        }
    }
}
