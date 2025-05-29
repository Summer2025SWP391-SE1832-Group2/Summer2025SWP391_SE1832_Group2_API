using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.DTO
{
    public class FavoriteDTO
    {
        public int FavoriteId { get; set; }

        public int? UserId { get; set; }

        public int? BlogId { get; set; }
    }
}
