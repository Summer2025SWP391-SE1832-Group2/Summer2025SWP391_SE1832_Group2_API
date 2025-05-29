using BDNAT_Repository.Entities;
using BDNAT_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.Implementation
{
    public class TestKitRepo : GenericRepository<TestKit>, ITestKitRepo
    {
        private static TestKitRepo _instance;

        public static TestKitRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TestKitRepo();
                }
                return _instance;
            }
        }
    }
}
