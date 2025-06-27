using RentNest.Core.Domains;
using RentNest.Infrastructure.Repositories.AccommodationRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Services.AccommodationService
{
    public class AccommodationService : IAccommodationService
    {
        private readonly IAccommodationRepository _accommodationRepository;

        public AccommodationService(IAccommodationRepository accommodationRepository)
        {
            _accommodationRepository = accommodationRepository;
        }
        public async Task<List<Post>> GetAccommodationsBySearchDto(string provinceName,
            string districtName,
            string? wardName,
            double? area,
            decimal? minMoney,
            decimal? maxMoney)
        {
            return await _accommodationRepository.GetAccommodationsBySearchDto(provinceName, districtName, wardName, area, minMoney, maxMoney);
        }
        public async Task<string> GetAccommodationImage(int accommodationId)
        {
            return await _accommodationRepository.GetAccommodationImage(accommodationId);
        }
        public async Task<string> GetAccommodationType(int accommodationId)
        {
            return await _accommodationRepository.GetAccommodationType(accommodationId);
        }
    }
}
