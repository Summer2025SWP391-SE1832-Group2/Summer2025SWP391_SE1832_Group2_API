using BDNAT_Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.Interface
{
    public interface IBlogRepo : IGenericRepository<Blog>
    {
        Task<List<Blog>> GetBlogsByBlogTypeIdAsync(int blogTypeId);
    }
}
