using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
using RentNest.Core.Utils;
using RentNest.Infrastructure.Repositories.PaymentRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.AccommodationRepo
{
    public class AccommodationRepository : RepositoryBase<Accommodation>, IAccommodationRepository
    {
        public AccommodationRepository(Db21027Context context) : base(context) { }

        public async Task<List<Post>> GetAccommodationsBySearchDto(
        string provinceName,
        string districtName,
        string? wardName,
        double? area,
        decimal? minMoney,
        decimal? maxMoney)
        {
            var query = _context.Posts
                .Include(p => p.Accommodation)
                    .ThenInclude(a => a.AccommodationImages)
                .Include(p => p.Accommodation)
                    .ThenInclude(a => a.AccommodationDetail)
                .Include(p => p.PostPackageDetails)
                    .ThenInclude(p => p.Pricing)
                        .ThenInclude(t => t.PackageType)
                .Include(p => p.PostPackageDetails)
                    .ThenInclude(d => d.Pricing)
                        .ThenInclude(p => p.TimeUnit)
                .Where(p => p.CurrentStatus == "A" && p.Accommodation.Status != "I")
                .OrderByDescending(p => p.PublishedAt)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(provinceName)
                || !string.IsNullOrWhiteSpace(districtName)
                || !string.IsNullOrWhiteSpace(wardName))
            {
                string Normalize(string? input)
                    => ProvinceNameNormalizer.Normalize(input ?? "").Trim();
                const string PlaceholderWard = "Chọn phường/xã...";
                const string PlaceholderDistrict = "Chọn quận/huyện...";
                const string PlaceholderProvince = "Chọn tỉnh/thành phố...";

                string normWard = Normalize(wardName);
                string normDistrict = Normalize(districtName);
                string normProvince = Normalize(provinceName);

                normWard = normWard == PlaceholderWard ? "" : normWard;
                normDistrict = normDistrict == PlaceholderDistrict ? "" : normDistrict;
                normProvince = normProvince == PlaceholderProvince ? "" : normProvince;

                var searchAddress = $"{normWard} {normDistrict} {normProvince}".Trim();
                var keyword = $"%{searchAddress}%";

                query = query.Where(p =>
                    EF.Functions.Like(
                        EF.Functions.Collate(
                            (
                                (p.Accommodation.WardName ?? "") + " " +
                                (p.Accommodation.DistrictName ?? "") + " " +
                                (p.Accommodation.ProvinceName ?? "")
                            ), "Vietnamese_CI_AI"
                        ), keyword)
                );
            }

            if (area.HasValue)
            {
                query = query.Where(p => p.Accommodation.Area >= area.Value);
            }

            if (minMoney.HasValue)
            {
                query = query.Where(p => p.Accommodation.Price >= minMoney.Value);
            }

            if (maxMoney.HasValue)
            {
                query = query.Where(p => p.Accommodation.Price <= maxMoney.Value);
            }

            Console.WriteLine(query.ToQueryString());

            return await query.ToListAsync();
        }

        public async Task<string> GetAccommodationImage(int accommodationId)
        {
            var roomImages = await _context.AccommodationImages.Where(i => i.AccommodationId == accommodationId).ToListAsync();
            return roomImages!.Select(i => i.ImageUrl).FirstOrDefault()!;
        }
        public async Task<string> GetAccommodationType(int accommodationId)
        {
            var roomType = await _context.AccommodationTypes.Where(t => t.TypeId == accommodationId).FirstOrDefaultAsync();
            return roomType!.TypeName;
        }
    }
}
