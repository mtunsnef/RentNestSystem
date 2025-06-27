using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.AmenityRepo
{
    public class AmenityRepository : RepositoryBase<Amenity>, IAmenityRepository
    {
        public AmenityRepository(Db21027Context context) : base(context)
        {
        }
    }
}
