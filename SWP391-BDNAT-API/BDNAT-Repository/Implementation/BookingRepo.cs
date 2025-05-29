using BDNAT_Repository.Entities;
using BDNAT_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.Implementation
{
    public class BookingRepo : GenericRepository<Booking>, IBookingRepo
    {
        private static BookingRepo _instance;

        public static BookingRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BookingRepo();
                }
                return _instance;
            }
        }
    }
}
