using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.AccommodationImageRepo
{
    public class AccommodationImageRepository : RepositoryBase<AccommodationImage>, IAccommodationImageRepository
    {
        public AccommodationImageRepository(Db21027Context context) : base(context)
        {
        }
    }
}
