using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.FavoriteRepo
{
    public class FavoriteRepository : RepositoryBase<FavoritePost>, IFavoriteRepository
    {
        public FavoriteRepository(Db21027Context context) : base(context)
        {
        }

        public async Task<List<FavoritePost>> GetFavoriteByUser(int accountId)
        {
            return await _dbSet
                            .Where(f => f.AccountId == accountId)
                            .Include(f => f.Post)
                                .ThenInclude(p => p.Accommodation)
                                    .ThenInclude(a => a.AccommodationDetail)
                            .Include(f => f.Post)
                                .ThenInclude(p => p.PostPackageDetails)
                                    .ThenInclude(p => p.Pricing)
                                        .ThenInclude(t => t.PackageType)
                            .Include(f => f.Post)
                                .ThenInclude(p => p.PostPackageDetails)
                                    .ThenInclude(d => d.Pricing)
                                        .ThenInclude(p => p.TimeUnit)
                            .Include(f => f.Post)
                                .ThenInclude(p => p.Accommodation)
                                    .ThenInclude(a => a.AccommodationImages)
                            .ToListAsync();
        }

        public async Task<bool> IsFavorite(int postId, int accountId)
        {
            return await _dbSet
                .AnyAsync(f => f.PostId == postId && f.AccountId == accountId);
        }

        public async Task RemoveFromFavorite(int postId, int accountId)
        {
            var favorite = await _dbSet
                .FirstOrDefaultAsync(f => f.PostId == postId && f.AccountId == accountId);

            if (favorite != null)
            {
                _context.FavoritePosts.Remove(favorite);
                _context.SaveChanges();
            }
        }
    }
}
