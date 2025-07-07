using Microsoft.EntityFrameworkCore.Storage;
using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.PostRepo
{
    public interface IPostRepository : IRepositoryBase<Post>
    {
        Task<List<Post>> GetAllPostsWithAccommodation();
        Task<List<Post>> GetTopVipPostsAsync();
        Task<Post?> GetPostDetailWithAccommodationDetailAsync(int postId);
        Task<List<Post>> GetAllPostsByUserAsync(int accountId);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
