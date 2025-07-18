﻿using BDNAT_Repository.Entities;
using BDNAT_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.Implementation
{
    public class ParameterRepo : GenericRepository<Parameter>, IParameterRepo
    {
        private static ParameterRepo _instance;

        public static ParameterRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ParameterRepo();
                }
                return _instance;
            }
        }
    }
}
