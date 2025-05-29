using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.DTO
{
    public class TestParameterDTO
    {
        public int TestParameterId { get; set; }

        public int? ServiceTypeId { get; set; }

        public int? ParameterId { get; set; }

        public int? DisplayOrder { get; set; }
    }
}
