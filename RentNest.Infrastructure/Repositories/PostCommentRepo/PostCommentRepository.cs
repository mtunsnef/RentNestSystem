using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
using RentNest.Infrastructure.Repositories.PostPackageDetailRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.PostCommentRepo
{
    public class PostCommentRepository : RepositoryBase<PostComment>, IPostCommentRepository
    {
        public PostCommentRepository(Db21027Context context) : base(context)
        {
        }

        public async Task<List<PostComment>> GetCommentsByPostIdAsync(int postId)
        {
            var comments = await _dbSet
                .Where(p => p.PostId == postId && p.ParentCommentId == null)
                .Include(c => c.InverseParentComment)
                    .ThenInclude(r => r.Account)
                        .ThenInclude(u => u.UserProfile)
                .Include(c => c.Account)
                    .ThenInclude(u => u.UserProfile)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return comments;
        }

    }
}
