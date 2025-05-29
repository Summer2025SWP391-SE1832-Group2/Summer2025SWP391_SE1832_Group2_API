using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.DTO
{
    public class ParameterDTO
    {
        public int ParameterId { get; set; }

        public string? Name { get; set; }

        public string? Unit { get; set; }

        public string? Description { get; set; }
    }
}
