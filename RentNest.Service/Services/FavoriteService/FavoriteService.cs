using RentNest.Core.Domains;
using RentNest.Infrastructure.Repositories.FavoriteRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Services.FavoriteService
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepo;

        public FavoriteService(IFavoriteRepository favoriteRepo)
        {
            _favoriteRepo = favoriteRepo;
        }
        public async Task AddToFavorite(int postId, int accountId)
        {
            await _favoriteRepo.AddAsync(new FavoritePost
            {
                PostId = postId,
                AccountId = accountId,
                CreatedAt = DateTime.Now
            });
        }

        public async Task<List<FavoritePost>> GetFavoriteByUser(int accountId)
        {
            return await _favoriteRepo.GetFavoriteByUser(accountId);
        }

        public async Task<bool> IsFavorite(int postId, int accountId)
        {
            return await _favoriteRepo.IsFavorite(postId, accountId);
        }

        public async Task RemoveFromFavorite(int postId, int accountId)
        {
            await _favoriteRepo.RemoveFromFavorite(postId, accountId);
        }
    }
}
