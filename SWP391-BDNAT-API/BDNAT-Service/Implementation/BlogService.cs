using AutoMapper;
using BDNAT_Repository.Entities;
using BDNAT_Repository.Implementation;
using BDNAT_Service.DTO;
using BDNAT_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Implementation
{
    public class BlogService : IBlogService
    {
        private readonly IMapper _mapper;

        public BlogService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateBlogAsync(BlogDTO blog)
        {
            var mapBlog = _mapper.Map<Blog>(blog);
            return await BlogRepo.Instance.InsertAsync(mapBlog);
        }

        public async Task<bool> DeleteBlogAsync(int id)
        {
            return await BlogRepo.Instance.DeleteAsync(id);
        }

        public async Task<List<BlogDTO>> GetAllBlogsAsync()
        {
            var BlogList = await BlogRepo.Instance.GetAllAsync();
            return BlogList.Select(log => _mapper.Map<BlogDTO>(log)).ToList();
        }

        public async Task<BlogDTO> GetBlogByIdAsync(int id)
        {
            return _mapper.Map<BlogDTO>(await BlogRepo.Instance.GetByIdAsync(id));
        }

        public async Task<bool> UpdateBlogAsync(BlogDTO blog)
        {
            var mapBlog = _mapper.Map<Blog>(blog);
            return await BlogRepo.Instance.UpdateAsync(mapBlog);
        }
    }
}
