using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.AccommodationAmenityRepo
{
    public class AccommodationAmenityRepository : RepositoryBase<AccommodationAmenity>, IAccommodationAmenityRepository
    {
        public AccommodationAmenityRepository(Db21027Context context) : base(context)
        {
        }
    }
}
