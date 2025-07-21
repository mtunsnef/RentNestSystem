using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.TimeUnitPackageRepo
{
    public interface ITimeUnitPackageRepository : IRepositoryBase<TimeUnitPackage>
    {
        Task<TimeUnitPackage?> GetTimeUnitById(int timeUnitId);
    }
}
