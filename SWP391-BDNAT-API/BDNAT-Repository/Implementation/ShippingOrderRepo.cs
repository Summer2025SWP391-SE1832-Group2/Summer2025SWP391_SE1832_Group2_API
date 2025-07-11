﻿using BDNAT_Repository.Entities;
using BDNAT_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.Implementation
{
    public class ShippingOrderRepo : GenericRepository<ShippingOrder>, IShippingOrderRepo
    {
        private static ShippingOrderRepo _instance;

        public static ShippingOrderRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ShippingOrderRepo();
                }
                return _instance;
            }
        }
    }
}
