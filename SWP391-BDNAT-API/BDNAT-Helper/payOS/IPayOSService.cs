using BDNAT_Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Helper.payOS
{
    public interface IPayOSService
    {
        public Task<string> RequestWithPayOsAsync(Booking order, decimal amount);
    }
}
