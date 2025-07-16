using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.PackagePricingRepo
{
    public interface IPackagePricingRepository : IRepositoryBase<PackagePricing>
    {
        Task<List<PackagePricing>> GetAllPackageOptions();
        Task<List<PostPackageType>> GetPackageTypesByTimeUnit(int timeUnitId);
        Task<decimal?> GetUnitPrice(int timeUnitId, int packageTypeId);
        Task<List<PackagePricing>> GetPackagePricingsByTimeUnitAndType(int timeUnitId, int packageTypeId);
        Task<int?> GetPricingIdAsync(int timeUnitId, int packageTypeId, int durationValue);
        Task<Dictionary<int, decimal>> GetAllUnitPrices(int timeUnitId);
    }
}
