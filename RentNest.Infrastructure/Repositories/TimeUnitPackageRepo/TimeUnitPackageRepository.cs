using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.TimeUnitPackageRepo
{
    public class TimeUnitPackageRepository : RepositoryBase<TimeUnitPackage>, ITimeUnitPackageRepository
    {
        public TimeUnitPackageRepository(Db21027Context context) : base(context)
        {
        }

        public async Task<TimeUnitPackage?> GetTimeUnitById(int timeUnitId)
        {
            return await _dbSet.FirstOrDefaultAsync(t => t.TimeUnitId == timeUnitId);
        }
    }
}
