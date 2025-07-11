﻿using BDNAT_Repository.Entities;
using BDNAT_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.Implementation
{
    public class ServiceTypeRepo : GenericRepository<ServiceType>, IServiceTypeRepo
    {
        private static ServiceTypeRepo _instance;

        public static ServiceTypeRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ServiceTypeRepo();
                }
                return _instance;
            }
        }
    }
}
