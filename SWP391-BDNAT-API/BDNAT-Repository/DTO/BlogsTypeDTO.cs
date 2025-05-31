using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.DTO
{
    public class BlogsTypeDTO
    {
        public int BlogTypeId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Img { get; set; }
    }
}
