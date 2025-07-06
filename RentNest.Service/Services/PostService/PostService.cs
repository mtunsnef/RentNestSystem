using Microsoft.AspNetCore.Http;
using RentNest.Core.Domains;
using RentNest.Core.Enums;
using RentNest.Core.Utils;
using RentNest.Infrastructure.Repositories.AccommodationAmenityRepo;
using RentNest.Infrastructure.Repositories.AccommodationDetailRepo;
using RentNest.Infrastructure.Repositories.AccommodationImageRepo;
using RentNest.Infrastructure.Repositories.AccommodationRepo;
using RentNest.Infrastructure.Repositories.PostPackageDetailRepo;
using RentNest.Infrastructure.Repositories.PostRepo;
using RentNest.Infrastructure.Repositories.PromoUsageRepo;
using RentNest.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IAccommodationAmenityRepository _accommodationAmenityRepository;
        private readonly IAccommodationDetailRepository _accommodationDetailRepository;
        private readonly IAccommodationImageRepository _accommodationImageRepository;
        private readonly IPostPackageDetailRepository _postPackageDetailRepository;
        private readonly IPromoUsageRepository _promoUsageRepository;
        public PostService(IPostRepository postRepository, IAccommodationRepository accommodationRepository, 
            IAccommodationImageRepository accommodationImageRepository, IAccommodationDetailRepository accommodationDetailRepository,
            IAccommodationAmenityRepository accommodationAmenityRepository, IPostPackageDetailRepository postPackageDetailRepository,
            IPromoUsageRepository promoUsageRepository)
        {
            _postRepository = postRepository;
            _accommodationRepository = accommodationRepository;
            _accommodationAmenityRepository = accommodationAmenityRepository;
            _accommodationDetailRepository = accommodationDetailRepository;
            _accommodationImageRepository = accommodationImageRepository;
            _postPackageDetailRepository = postPackageDetailRepository;
            _promoUsageRepository = promoUsageRepository;
        }
        public async Task<List<Post>> GetAllPostsByUserAsync(int accountId)
        {
            return await _postRepository.GetAllPostsByUserAsync(accountId);
        }

        public async Task<List<Post>> GetAllPostsWithAccommodation()
        {
            return await _postRepository.GetAllPostsWithAccommodation();
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            return await _postRepository.GetByIdAsync(id);
        }

        public async Task<Post?> GetPostDetailWithAccommodationDetailAsync(int postId)
        {
            return await _postRepository.GetPostDetailWithAccommodationDetailAsync(postId);
        }

        public async Task<List<Post>> GetTopVipPostsAsync()
        {
            return await _postRepository.GetTopVipPostsAsync();
        }

        public async Task<int> SavePost(LandlordPostDto dto)
        {
            using var transaction = await _postRepository.BeginTransactionAsync();

            try
            {
                var addressParts = (dto.Address ?? "").Split(',').Select(p => p.Trim()).ToArray();

                string streetAddress = addressParts.ElementAtOrDefault(0) ?? "";
                string wardName = addressParts.ElementAtOrDefault(1) ?? "";
                string districtName = addressParts.ElementAtOrDefault(2) ?? "";
                string provinceName = addressParts.ElementAtOrDefault(3) ?? "";

                var accommodation = new Accommodation
                {
                    Title = dto.AccommodationDescription ?? "",
                    Description = dto.AccommodationDescription ?? "",
                    Address = streetAddress,
                    WardName = wardName,
                    DistrictName = districtName,
                    ProvinceName = provinceName,
                    Price = dto.Price,
                    Area = dto.Area,
                    TypeId = dto.AccommodationTypeId,
                    OwnerId = dto.OwnerId ?? 0,
                };
                await _accommodationRepository.AddAsync(accommodation);

                var detail = new AccommodationDetail
                {
                    AccommodationId = accommodation.AccommodationId,
                    FurnitureStatus = dto.FurnitureStatus,
                    BedroomCount = dto.NumberBedroom ?? 0,
                    BathroomCount = dto.NumberBathroom ?? 0,
                    HasAirConditioner = dto.HasAirConditioner ?? false,
                    HasKitchenCabinet = dto.HasKitchenCabinet ?? false,
                    HasLoft = dto.HasLoft ?? false,
                    HasRefrigerator = dto.HasRefrigerator ?? false,
                    HasWashingMachine = dto.HasWashingMachine ?? false
                };
                await _accommodationDetailRepository.AddAsync(detail);

                if (dto.AmenityIds != null)
                {
                    foreach (var amenityId in dto.AmenityIds)
                    {
                        var aa = new AccommodationAmenity
                        {
                            AccommodationId = accommodation.AccommodationId,
                            AmenityId = amenityId
                        };
                        await _accommodationAmenityRepository.AddAsync(aa);
                    }
                }

                if (dto.Images != null)
                {
                    foreach (var image in dto.Images)
                    {
                        string imageUrl = await UploadImageAndGetUrlAsync(image);

                        var img = new AccommodationImage
                        {
                            AccommodationId = accommodation.AccommodationId,
                            ImageUrl = imageUrl,
                            Caption = null
                        };
                        await _accommodationImageRepository.AddAsync(img);
                    }
                }

                var post = new Post
                {
                    Title = dto.titlePost,
                    Content = dto.contentPost,
                    CurrentStatus = PostStatusHelper.ToDbValue(PostStatus.Unpaid),
                    PublishedAt = DateTime.Now,
                    AccountId = dto.OwnerId ?? 0,
                    AccommodationId = accommodation.AccommodationId
                };
                await _postRepository.AddAsync(post);

                var postpackageDetail = new PostPackageDetail
                {
                    PostId = post.PostId,
                    PricingId = dto.PricingId ?? 0,
                    TotalPrice = (decimal)(dto.TotalPrice),
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    PaymentStatus = PaymentStatusHelper.ToDbValue(PaymentStatus.Pending),
                    PaymentTransactionId = null,
                    CreatedAt = DateTime.Now,
                };
                await _postPackageDetailRepository.AddAsync(postpackageDetail);

                if (dto.PromotionId.HasValue)
                {
                    var promoUsage = new PromoUsage
                    {
                        AccountId = dto.OwnerId ?? 0,
                        PromoId = dto.PromotionId.Value,
                        UsedAt = DateTime.Now,
                        PostId = post.PostId
                    };
                    await _promoUsageRepository.AddAsync(promoUsage);
                }

                await transaction.CommitAsync();

                return post.PostId;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateAsync(Post post)
        {
            await _postRepository.UpdateAsync(post);
        }

        private async Task<string> UploadImageAndGetUrlAsync(IFormFile file)
        {
            var filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var uploadFolder = Path.Combine("wwwroot", "uploads");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }
            var fullPath = Path.Combine(uploadFolder, filename);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return "/uploads/" + filename;
        }
    }
}
