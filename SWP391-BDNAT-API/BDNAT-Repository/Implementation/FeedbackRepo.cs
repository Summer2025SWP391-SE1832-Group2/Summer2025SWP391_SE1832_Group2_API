using BDNAT_Repository.Entities;
using BDNAT_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.Implementation
{
    public class FeedbackRepo : GenericRepository<Feedback>, IFeedbackRepo
    {
        private static FeedbackRepo _instance;

        public static FeedbackRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FeedbackRepo();
                }
                return _instance;
            }
        }
    }
}
