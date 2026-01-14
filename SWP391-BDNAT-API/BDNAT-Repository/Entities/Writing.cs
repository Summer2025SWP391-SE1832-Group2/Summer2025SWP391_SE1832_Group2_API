using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.Entities
{
    internal class Writing
    {
            public int WritingId { get; set; }
            public string Title { get; set; } = string.Empty;
            public string Content { get; set; } = string.Empty;
            public DateTime CreatedAt { get; set; } = DateTime.Now; // lấy thời gian hiện tại làm mặc định
    }
}
