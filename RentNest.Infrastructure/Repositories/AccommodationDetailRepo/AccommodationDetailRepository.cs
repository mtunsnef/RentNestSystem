using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.AccommodationDetailRepo
{
    public class AccommodationDetailRepository : RepositoryBase<AccommodationDetail>, IAccommodationDetailRepository
    {
        public AccommodationDetailRepository(Db21027Context context) : base(context)
        {
        }
    }
}
