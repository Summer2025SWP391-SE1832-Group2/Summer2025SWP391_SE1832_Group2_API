using BDNAT_Repository.Entities;
using BDNAT_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.Implementation
{
    public class ResultRepo : GenericRepository<Result>, IResultRepo
    {
        private static ResultRepo _instance;

        public static ResultRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ResultRepo();
                }
                return _instance;
            }
        }
    }
}
