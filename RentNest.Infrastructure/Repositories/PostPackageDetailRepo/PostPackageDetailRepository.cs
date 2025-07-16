using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.PostPackageDetailRepo
{
    public class PostPackageDetailRepository : RepositoryBase<PostPackageDetail>, IPostPackageDetailRepository
    {
        public PostPackageDetailRepository(Db21027Context context) : base(context)
        {
        }

        public async Task<PostPackageDetail?> GetByPostIdAsync(int postId)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(p => p.Pricing)
                    .ThenInclude(o => o.PackageType)
                .FirstOrDefaultAsync(p => p.PostId == postId);
        }
    }
}
