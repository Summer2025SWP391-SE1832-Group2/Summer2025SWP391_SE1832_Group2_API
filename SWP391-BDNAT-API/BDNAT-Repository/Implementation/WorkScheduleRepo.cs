using BDNAT_Repository.Entities;
using BDNAT_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.Implementation
{
    public class WorkScheduleRepo : GenericRepository<WorkSchedule>, IWorkScheduleRepo
    {
        private static WorkScheduleRepo _instance;

        public static WorkScheduleRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WorkScheduleRepo();
                }
                return _instance;
            }
        }
    }
}
