using BDNAT_Repository.Entities;
using BDNAT_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.Implementation
{
    public class KitOrderRepo : GenericRepository<KitOrder>, IKitOrderRepo
    {
        private static KitOrderRepo _instance;

        public static KitOrderRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new KitOrderRepo();
                }
                return _instance;
            }
        }
    }
}
