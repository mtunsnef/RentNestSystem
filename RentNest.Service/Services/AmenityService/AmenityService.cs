using RentNest.Core.Domains;
using RentNest.Infrastructure.Repositories.AmenityRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Services.AmenityService
{
    public class AmenityService : IAmenityService
    {
        private readonly IAmenityRepository _amenitiesRepository;

        public AmenityService(IAmenityRepository amenitiesRepository)
        {
            _amenitiesRepository = amenitiesRepository;
        }
        public async Task<IEnumerable<Amenity>> GetAll()
        {
            return await _amenitiesRepository.GetAllAsync();
        }
    }
}
