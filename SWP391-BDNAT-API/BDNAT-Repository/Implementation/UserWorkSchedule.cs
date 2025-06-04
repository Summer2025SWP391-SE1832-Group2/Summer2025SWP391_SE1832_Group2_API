using BDNAT_Repository.Entities;
using BDNAT_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.Implementation
{
    public class UserWorkScheduleRepo : GenericRepository<UserWorkSchedule>, IUserWorkScheduleRepo
    {
        private static UserWorkScheduleRepo _instance;

        public static UserWorkScheduleRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserWorkScheduleRepo();
                }
                return _instance;
            }
        }
    }
}
