using BDNAT_Repository.Entities;
using BDNAT_Repository.Interface;
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

        public async Task<List<SampleCollectionSchedule>> GetAllWhereCollectorIsNullAsync()
        {
            var allSchedules = await GetAllAsync();  // Lấy tất cả bản ghi
            var result = allSchedules.Where(s => s.CollectorId == null).ToList();
            return result;
        }
    }
}
