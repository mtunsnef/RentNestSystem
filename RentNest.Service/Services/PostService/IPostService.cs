using RentNest.Core.Domains;
using RentNest.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Services.PostService
{
    public interface IPostService
    {
        Task<List<Post>> GetAllPostsByUserAsync(int accountId);
        Task<List<Post>> GetAllPostsWithAccommodation();
        Task<Post?> GetPostDetailWithAccommodationDetailAsync(int postId);
        Task<List<Post>> GetTopVipPostsAsync();
        Task<int> SavePost(LandlordPostDto dto);
        Task<Post?> GetByIdAsync(int id);
        Task UpdateAsync(Post post);
    }
}
