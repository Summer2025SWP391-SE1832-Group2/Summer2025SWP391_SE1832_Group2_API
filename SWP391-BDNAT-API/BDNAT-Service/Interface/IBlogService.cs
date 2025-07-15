using BDNAT_Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Interface
{
    public interface IBlogService
    {
        Task<List<BlogDTO>> GetAllBlogsAsync();

        Task<List<BlogDTO>> GetBlogsByBlogTypeIdAsync(int BlogTypeId);

        Task<BlogDTO> GetBlogByIdAsync(int id);
        Task<bool> CreateBlogAsync(BlogDTO blog);
        Task<bool> UpdateBlogAsync(BlogDTO blog);
        Task<bool> DeleteBlogAsync(int id);
        Task<List<BlogDTO>> GetFavoriteBlogsByUserId(int userId);
    }

}
