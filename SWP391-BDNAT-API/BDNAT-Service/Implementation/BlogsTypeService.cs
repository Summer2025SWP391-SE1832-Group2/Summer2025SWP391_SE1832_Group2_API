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
    public class BlogsTypeService : IBlogsTypeService
    {
        private readonly IMapper _mapper;

        public BlogsTypeService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateBlogsTypeAsync(BlogsTypeDTO blogsType)
        {
            var map = _mapper.Map<BlogsType>(blogsType);
            return await BlogsTypeRepo.Instance.InsertAsync(map);
        }

        public async Task<bool> DeleteBlogsTypeAsync(int id)
        {
            return await BlogsTypeRepo.Instance.DeleteAsync(id);
        }

        public async Task<List<BlogsTypeDTO>> GetAllBlogsTypesAsync()
        {
            var list = await BlogsTypeRepo.Instance.GetAllAsync();
            return list.Select(x => _mapper.Map<BlogsTypeDTO>(x)).ToList();
        }

        public async Task<BlogsTypeDTO> GetBlogsTypeByIdAsync(int id)
        {
            return _mapper.Map<BlogsTypeDTO>(await BlogsTypeRepo.Instance.GetByIdAsync(id));
        }

        public async Task<bool> UpdateBlogsTypeAsync(BlogsTypeDTO blogsType)
        {
            var map = _mapper.Map<BlogsType>(blogsType);
            return await BlogsTypeRepo.Instance.UpdateAsync(map);
        }
    }

}
