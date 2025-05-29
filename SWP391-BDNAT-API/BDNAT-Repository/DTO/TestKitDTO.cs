using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.DTO
{
    public class TestKitDTO
    {
        public int TestKitId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public string? IncludeItems { get; set; }

        public bool? IsActive { get; set; }
    }
}
