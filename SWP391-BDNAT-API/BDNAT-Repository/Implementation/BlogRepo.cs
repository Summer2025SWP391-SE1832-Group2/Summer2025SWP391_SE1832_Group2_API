using BDNAT_Repository.Entities;
using BDNAT_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.Implementation
{
    public class BlogRepo : GenericRepository<Blog>, IBlogRepo
    {
        private static BlogRepo _instance;

        public static BlogRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BlogRepo();
                }
                return _instance;
            }
        }
    }
}
