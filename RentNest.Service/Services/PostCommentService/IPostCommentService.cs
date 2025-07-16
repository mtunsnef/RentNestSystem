using RentNest.Core.Domains;
using RentNest.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Services.PostCommentService
{
    public interface IPostCommentService
    {
        Task<List<PostCommentDto>> GetCommentsByPostIdAsync(int postId);
        Task AddCommentAsync(PostComment comment);
    }
}
