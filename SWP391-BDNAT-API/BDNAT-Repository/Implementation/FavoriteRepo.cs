using BDNAT_Repository.Entities;
using BDNAT_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.Implementation
{
    public class FavoriteRepo : GenericRepository<Favorite>, IFavoriteRepo
    {
        private static FavoriteRepo _instance;

        public static FavoriteRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FavoriteRepo();
                }
                return _instance;
            }
        }
    }
}
