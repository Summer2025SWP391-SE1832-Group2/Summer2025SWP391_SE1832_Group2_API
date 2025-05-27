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
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;

        public CommentService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateCommentAsync(CommentDTO comment)
        {
            var map = _mapper.Map<Comment>(comment);
            return await CommentRepo.Instance.InsertAsync(map);
        }

        public async Task<bool> DeleteCommentAsync(int id)
        {
            return await CommentRepo.Instance.DeleteAsync(id);
        }

        public async Task<List<CommentDTO>> GetAllCommentsAsync()
        {
            var list = await CommentRepo.Instance.GetAllAsync();
            return list.Select(x => _mapper.Map<CommentDTO>(x)).ToList();
        }

        public async Task<CommentDTO> GetCommentByIdAsync(int id)
        {
            return _mapper.Map<CommentDTO>(await CommentRepo.Instance.GetByIdAsync(id));
        }

        public async Task<bool> UpdateCommentAsync(CommentDTO comment)
        {
            var map = _mapper.Map<Comment>(comment);
            return await CommentRepo.Instance.UpdateAsync(map);
        }
    }
}
