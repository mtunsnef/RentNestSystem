using RentNest.Core.Domains;
using RentNest.Infrastructure.Repositories.PackagePricingRepo;
using RentNest.Infrastructure.Repositories.TimeUnitPackageRepo;
using RentNest.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Services.PackagePricingService
{
    public class PackagePricingService : IPackagePricingService
    {
        private readonly IPackagePricingRepository _packagePricingRepository;
        private readonly ITimeUnitPackageRepository _timeUnitPackageRepository;
        public PackagePricingService(IPackagePricingRepository packagePricingRepository, ITimeUnitPackageRepository timeUnitPackageRepository)
        {
            _packagePricingRepository = packagePricingRepository;
            _timeUnitPackageRepository = timeUnitPackageRepository;
        }

        public async Task<IEnumerable<PackagePricing>> GetAllPackageOptions()
        {
            return await _packagePricingRepository.GetAllPackageOptions();
        }

        public async Task<List<DurationPriceDto>> GetDurationsAndPrices(int timeUnitId, int packageTypeId)
        {
            var pricings = await _packagePricingRepository.GetPackagePricingsByTimeUnitAndType(timeUnitId, packageTypeId);

            return pricings.Select(p => new DurationPriceDto
            {
                DurationValue = p.DurationValue,
                UnitPrice = p.UnitPrice,
                TotalPrice = p.TotalPrice,
                TimeUnitName = p.TimeUnit?.TimeUnitName
            }).ToList();
        }

        //public async Task<List<PackageTypeDto>> GetPackageTypes(int timeUnitId)
        //{
        //    var packageTypes = await _packagePricingRepository.GetPackageTypesByTimeUnit(timeUnitId);
        //    var timeUnit = await _timeUnitPackageRepository.GetTimeUnitById(timeUnitId);

        //    var result = new List<PackageTypeDto>();

        //    foreach (var pt in packageTypes)
        //    {
        //        var unitPrice = await _packagePricingRepository.GetUnitPrice(timeUnitId, pt.PackageTypeId);

        //        result.Add(new PackageTypeDto
        //        {
        //            PackageTypeId = pt.PackageTypeId,
        //            PackageTypeName = pt.PackageTypeName,
        //            Description = pt.Description,
        //            TimeUnitName = timeUnit?.TimeUnitName,
        //            UnitPrice = unitPrice ?? 0
        //        });
        //    }

        //    return result;
        //}

        public async Task<List<PackageTypeDto>> GetPackageTypes(int timeUnitId)
        {
            var packageTypes = await _packagePricingRepository.GetPackageTypesByTimeUnit(timeUnitId);
            var timeUnit = await _timeUnitPackageRepository.GetTimeUnitById(timeUnitId);
            var unitPriceMap = await _packagePricingRepository.GetAllUnitPrices(timeUnitId);

            var result = packageTypes.Select(pt => new PackageTypeDto
            {
                PackageTypeId = pt.PackageTypeId,
                PackageTypeName = pt.PackageTypeName,
                Description = pt.Description,
                TimeUnitName = timeUnit?.TimeUnitName,
                UnitPrice = unitPriceMap.TryGetValue(pt.PackageTypeId, out var price) ? price : 0
            }).ToList();

            return result;
        }


        public async Task<List<PostPackageType>> GetPackageTypesByTimeUnit(int timeUnitId)
        {
            return await _packagePricingRepository.GetPackageTypesByTimeUnit(timeUnitId);
        }

        public async Task<int?> GetPricingIdAsync(int timeUnitId, int packageTypeId, int durationValue)
        {
            return await _packagePricingRepository.GetPricingIdAsync(timeUnitId, packageTypeId, durationValue);
        }
    }
}
