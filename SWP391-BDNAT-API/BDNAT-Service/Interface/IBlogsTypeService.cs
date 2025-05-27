using BDNAT_Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Interface
{
    public interface IBlogsTypeService
    {
        Task<List<BlogsTypeDTO>> GetAllBlogsTypesAsync();
        Task<BlogsTypeDTO> GetBlogsTypeByIdAsync(int id);
        Task<bool> CreateBlogsTypeAsync(BlogsTypeDTO blogsType);
        Task<bool> UpdateBlogsTypeAsync(BlogsTypeDTO blogsType);
        Task<bool> DeleteBlogsTypeAsync(int id);
    }
}
