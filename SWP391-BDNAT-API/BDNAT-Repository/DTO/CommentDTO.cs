using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.DTO
{
    public class CommentDTO
    {
        public int UniqueId { get; set; }

        public int UserId { get; set; }

        public int BlogId { get; set; }

        public string Comment1 { get; set; } = null!;

        public int? RootId { get; set; }
    }
}
