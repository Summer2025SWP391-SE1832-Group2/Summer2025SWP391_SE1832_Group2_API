using BDNAT_Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.DTO
{
    public class BlogDTO
    {
        public int BlogId { get; set; }

        public int? CreateBy { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        public int? BlogTypeId { get; set; }
    }
}
