using BDNAT_Repository.Entities;
using BDNAT_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.Implementation
{
    public class CommentRepo : GenericRepository<Comment>, ICommentRepo
    {
        private static CommentRepo _instance;

        public static CommentRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CommentRepo();
                }
                return _instance;
            }
        }
    }
}
