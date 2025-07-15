using BDNAT_Repository.Entities;
using BDNAT_Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.Implementation
{
    public class CommentRepo : GenericRepository<Comment>, ICommentRepo
    {
        private static CommentRepo _instance;

        public static CommentRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CommentRepo();
                }
                return _instance;
            }
        }

        public async Task<List<Comment>> GetAllCommentsByBlogIdAsync(int blogId)
        {
            var comments = await _context.Comments
                .Where(c => c.BlogId == blogId)
                .Include(c => c.User)
                .Include(c => c.Root)
                .ToListAsync();

            return comments;
        }

        public async Task<bool> CreateCommentAsync(Comment comment)
        {
            try
            {
                if (comment.RootId.HasValue)
                {
                    // Kiểm tra xem RootId có tồn tại không
                    var rootComment = await _context.Comments.FindAsync(comment.RootId.Value);
                    if (rootComment == null)
                    {
                        Console.WriteLine($"Root comment with ID {comment.RootId.Value} does not exist.");
                        return false;
                    }
                }

                await _context.Comments.AddAsync(comment);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating comment: {ex.Message}");
                return false;
            }
        }

    }
}
