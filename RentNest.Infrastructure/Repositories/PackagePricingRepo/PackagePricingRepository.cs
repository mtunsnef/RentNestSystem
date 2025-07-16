using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.PackagePricingRepo
{
    public class PackagePricingRepository : RepositoryBase<PackagePricing>, IPackagePricingRepository
    {
        public PackagePricingRepository(Db21027Context context) : base(context)
        {
        }

        public async Task<List<PackagePricing>> GetAllPackageOptions()
        {
            return await _dbSet
                .Include(t => t.TimeUnit)
                .Include(p => p.PackageType)
                .OrderBy(t => t.TimeUnit)
                .ThenBy(p => p.PackageType.Priority)
                .ToListAsync();
        }

        public async Task<List<PackagePricing>> GetPackagePricingsByTimeUnitAndType(int timeUnitId, int packageTypeId)
        {
            return await _dbSet
                .Include(p => p.TimeUnit)
                .Where(p => p.TimeUnitId == timeUnitId && p.PackageTypeId == packageTypeId)
                .OrderBy(p => p.DurationValue)
                .ToListAsync();
        }

        public async Task<List<PostPackageType>> GetPackageTypesByTimeUnit(int timeUnitId)
        {
            return await _dbSet
                .Where(p => p.TimeUnitId == timeUnitId)
                .Select(p => p.PackageType)
                .Distinct()
                .OrderBy(pt => pt.Priority)
                .ToListAsync();
        }

        public async Task<int?> GetPricingIdAsync(int timeUnitId, int packageTypeId, int durationValue)
        {
            var pricing = await _dbSet
                .Where(p => p.TimeUnitId == timeUnitId
                            && p.PackageTypeId == packageTypeId
                            && p.DurationValue == durationValue
                            && p.IsActive == true)
                .Select(p => p.PricingId)
                .FirstOrDefaultAsync();

            return pricing == 0 ? null : pricing;
        }

        public async Task<decimal?> GetUnitPrice(int timeUnitId, int packageTypeId)
        {
            return await _dbSet
                .Where(p => p.TimeUnitId == timeUnitId && p.PackageTypeId == packageTypeId)
                .Select(p => p.UnitPrice)
                .FirstOrDefaultAsync();
        }

        public async Task<Dictionary<int, decimal>> GetAllUnitPrices(int timeUnitId)
        {
            return await _dbSet
                .Where(p => p.TimeUnitId == timeUnitId && p.IsActive == true)
                .GroupBy(p => p.PackageTypeId)
                .Select(g => new
                {
                    PackageTypeId = g.Key,
                    UnitPrice = g.OrderBy(p => p.DurationValue).FirstOrDefault().UnitPrice
                })
                .ToDictionaryAsync(x => x.PackageTypeId, x => x.UnitPrice);
        }

    }
}
