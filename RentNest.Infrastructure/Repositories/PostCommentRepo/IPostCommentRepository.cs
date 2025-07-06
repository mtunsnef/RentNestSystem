using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.PostCommentRepo
{
    public interface IPostCommentRepository : IRepositoryBase<PostComment>
    {
        Task<List<PostComment>> GetCommentsByPostIdAsync(int postId);
    }
}
