using BDNAT_Repository.Entities;
using BDNAT_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.Implementation
{
    public class ServiceRepo : GenericRepository<Service>, IServiceRepo
    {
        private static ServiceRepo _instance;

        public static ServiceRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ServiceRepo();
                }
                return _instance;
            }
        }
    }
}
