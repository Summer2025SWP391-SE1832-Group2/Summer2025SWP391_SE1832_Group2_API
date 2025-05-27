using BDNAT_Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Interface
{
    public interface ICommentService
    {
        Task<List<CommentDTO>> GetAllCommentsAsync();
        Task<CommentDTO> GetCommentByIdAsync(int id);
        Task<bool> CreateCommentAsync(CommentDTO comment);
        Task<bool> UpdateCommentAsync(CommentDTO comment);
        Task<bool> DeleteCommentAsync(int id);
    }
}
