﻿using RentNest.Core.Domains;
using RentNest.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Services.PackagePricingService
{
    public interface IPackagePricingService
    {
        Task<IEnumerable<PackagePricing>> GetAllPackageOptions();
        Task<List<PostPackageType>> GetPackageTypesByTimeUnit(int timeUnitId);
        Task<List<DurationPriceDto>> GetDurationsAndPrices(int timeUnitId, int packageTypeId);
        Task<List<PackageTypeDto>> GetPackageTypes(int timeUnitId);
        Task<int?> GetPricingIdAsync(int timeUnitId, int packageTypeId, int durationValue);
    }
}
