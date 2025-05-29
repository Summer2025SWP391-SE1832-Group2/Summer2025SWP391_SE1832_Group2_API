using BDNAT_Repository.Entities;
using BDNAT_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.Implementation
{
    public class BlogsTypeRepo : GenericRepository<BlogsType>, IBlogsTypeRepo
    {
        private static BlogsTypeRepo _instance;

        public static BlogsTypeRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BlogsTypeRepo();
                }
                return _instance;
            }
        }
    }
}
