using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Services.FavoriteService
{
    public interface IFavoriteService
    {
        Task AddToFavorite(int postId, int accountId);
        Task<bool> IsFavorite(int postId, int accountId);
        Task RemoveFromFavorite(int postId, int accountId);
        Task<List<FavoritePost>> GetFavoriteByUser(int accountId);
    }
}
