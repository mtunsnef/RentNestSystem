using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentNest.Core.Consts;
using RentNest.Core.Domains;
using RentNest.Core.Utils;
using RentNest.Service.DTOs;
using RentNest.Service.Services.AccommodationService;
using RentNest.Service.Services.AccountService;
using RentNest.Service.Services.AmenityService;
using RentNest.Service.Services.ConversationService;
using RentNest.Service.Services.FavoriteService;
using RentNest.Service.Services.PostCommentService;
using RentNest.Service.Services.PostService;
using RentNest.Web.Models;

namespace RentNest.Web.Controllers
{
    public class AccommodationsController : Controller
    {
        private readonly IAccommodationService _accommodationService;
        private readonly IPostService _postService;
        private readonly IAmenityService _amenitiesSerivce;
        private readonly IFavoriteService _favoriteService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConversationService _conversationService;
        private readonly IPostCommentService _postCommentService;
        private readonly IAccountService _accountService;

        public AccommodationsController(IConfiguration configuration, IAccommodationService accommodationService, IPostService postService,
        IFavoriteService favoriteService, IAmenityService amenitiesSerivce, IHttpContextAccessor httpContextAccessor, IConversationService conversationService,
        IPostCommentService postCommentService, IAccountService accountService)
        {
            _accommodationService = accommodationService;
            _postService = postService;
            _amenitiesSerivce = amenitiesSerivce;
            _favoriteService = favoriteService;
            _httpContextAccessor = httpContextAccessor;
            _conversationService = conversationService;
            _postCommentService = postCommentService;
            _accountService = accountService;
        }

        [HttpGet]
        [Route("danh-sach-phong-tro")]
        public async Task<IActionResult> Index(string? roomType, string? furnitureStatus, int? bedroomCount, int? bathroomCount)
        {
            string? provinceName = TempData["provinceName"] as string;
            string? districtName = TempData["districtName"] as string;
            string? wardName = TempData["wardName"] as string;
            double? area = double.TryParse(TempData["area"] as string, out var a) ? a : null;
            decimal? minMoney = decimal.TryParse(TempData["minMoney"] as string, out var min) ? min : null;
            decimal? maxMoney = decimal.TryParse(TempData["maxMoney"] as string, out var max) ? max : null;

            ViewBag.ProvinceName = provinceName;
            ViewBag.DistrictName = districtName;
            ViewBag.WardName = wardName;
            ViewBag.Area = area;
            ViewBag.MinMoney = minMoney;
            ViewBag.MaxMoney = maxMoney;

            List<AccommodationIndexViewModel> model;

            if (TempData["RoomListSearch"] != null)
            {
                model = JsonConvert.DeserializeObject<List<AccommodationIndexViewModel>>(TempData["RoomListSearch"]!.ToString()!)!;
            }
            else
            {
                var posts = await _postService.GetAllPostsWithAccommodation();
                model = posts.Select(p =>
                {
                    var latestPackageDetail = p.PostPackageDetails
                        .OrderByDescending(ppd => ppd.CreatedAt)
                        .FirstOrDefault();

                    return new AccommodationIndexViewModel
                    {
                        Id = p.PostId,
                        Status = p.CurrentStatus,
                        Title = p.Title,
                        Price = p.Accommodation.Price,
                        Address = p.Accommodation.Address,
                        Area = p.Accommodation.Area,
                        BathroomCount = p.Accommodation?.AccommodationDetail?.BathroomCount,
                        BedroomCount = p.Accommodation?.AccommodationDetail?.BedroomCount,
                        ImageUrl = p.Accommodation?.AccommodationImages?.FirstOrDefault()?.ImageUrl ?? "default-image.jpg",
                        CreatedAt = p.CreatedAt,
                        DistrictName = p.Accommodation?.DistrictName ?? "",
                        ProvinceName = p.Accommodation?.ProvinceName ?? "",
                        WardName = p.Accommodation?.WardName ?? "",
                        PackageTypeName = latestPackageDetail?.Pricing?.PackageType?.PackageTypeName ?? "",
                        TimeUnitName = latestPackageDetail?.Pricing?.TimeUnit?.TimeUnitName ?? "",
                        TotalPrice = latestPackageDetail?.TotalPrice ?? 0,
                        StartDate = latestPackageDetail?.StartDate,
                        EndDate = latestPackageDetail?.EndDate,
                        ListImages = p.Accommodation?.AccommodationImages?.Select(i => i.ImageUrl).ToList() ?? new List<string>(),
                        PhoneNumber = p.Account.UserProfile?.PhoneNumber ?? ""
                    };
                }).ToList();
            }
            if (!string.IsNullOrEmpty(roomType))
            {
                model = model
                    .Where(m => !string.IsNullOrEmpty(m.Title) && m.Title.Contains(roomType, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(furnitureStatus))
            {
                model = model.Where(m => m.Status == furnitureStatus).ToList();
            }

            if (bedroomCount.HasValue)
            {
                model = model.Where(m => m.BedroomCount == bedroomCount).ToList();
            }

            if (bathroomCount.HasValue)
            {
                model = model.Where(m => m.BathroomCount == bathroomCount).ToList();
            }

            bool hasSearched =
                !string.IsNullOrEmpty(roomType) ||
                !string.IsNullOrEmpty(furnitureStatus) ||
                bedroomCount.HasValue ||
                bathroomCount.HasValue ||
                ((TempData["HasSearched"] as bool?) ?? false);

            ViewBag.HasSearched = hasSearched;
            ViewBag.RoomType = roomType;
            ViewBag.FurnitureStatus = furnitureStatus;
            ViewBag.BedroomCount = bedroomCount;
            ViewBag.BathroomCount = bathroomCount;

            return View(model);
        }

        [Authorize(AuthenticationSchemes = AuthSchemes.Cookie, Roles = $"{UserRoles.User},{UserRoles.Landlord}")]
        [Route("bai-viet-yeu-thich")]
        public async Task<IActionResult> FavoritePosts()
        {
            var accountId = User.GetUserId();
            if (accountId == 0)
            {
                return Unauthorized();
            }
            var favoritePosts = await _favoriteService.GetFavoriteByUser(accountId ?? 0);

            var viewModelList = favoritePosts.Select(f => new AccommodationIndexViewModel
            {
                Id = f.Post.PostId,
                Title = f.Post.Accommodation.Title,
                Address = f.Post.Accommodation.Address,
                Price = f.Post.Accommodation.Price,
                Status = f.Post.Accommodation.Status,
                ImageUrl = f.Post.Accommodation?.AccommodationImages?.FirstOrDefault()?.ImageUrl ?? "default-image.jpg",
                Area = f.Post?.Accommodation?.Area,
                BedroomCount = f.Post?.Accommodation?.AccommodationDetail?.BedroomCount,
                BathroomCount = f.Post?.Accommodation?.AccommodationDetail?.BathroomCount,
                CreatedAt = f.Post?.CreatedAt,
                DistrictName = f.Post?.Accommodation?.DistrictName ?? "",
                ProvinceName = f.Post?.Accommodation?.ProvinceName ?? "",
                WardName = f.Post?.Accommodation?.WardName ?? "",
                PackageTypeName = f.Post?.PostPackageDetails
                    .OrderByDescending(p => p.CreatedAt)
                    .Select(p => p.Pricing.PackageType.PackageTypeName)
                    .FirstOrDefault() ?? ""
            }).ToList();

            return View("FavoritePosts", viewModelList);
        }

        [HttpGet("chi-tiet/{postId:int}", Name = "PostDetailRoute")]
        public async Task<IActionResult> Detail(int postId)
        {
            var post = await _postService.GetPostDetailWithAccommodationDetailAsync(postId);

            if (post == null || post.Accommodation == null || post.Accommodation.AccommodationDetail == null)
            {
                return Content("Không tìm thấy dữ liệu chi tiết");
            }

            var imageUrls = post.Accommodation.AccommodationImages?
                .Select(img => img.ImageUrl)
                .ToList() ?? new List<string>();

            var latestPackage = post.PostPackageDetails
                 .OrderByDescending(p => p.StartDate)
                 .FirstOrDefault();

            var viewModel = new AccommodationDetailViewModel
            {
                PostId = post.PostId,
                PostTitle = post.Title,
                PostContent = post.Content,
                DetailId = post.Accommodation.AccommodationDetail.DetailId,
                AccommodationId = post.Accommodation.AccommodationId,
                ImageUrls = imageUrls,
                Price = post.Accommodation.Price,
                Description = post.Accommodation.Description,
                BathroomCount = post.Accommodation.AccommodationDetail.BathroomCount,
                BedroomCount = post.Accommodation.AccommodationDetail.BedroomCount,
                HasKitchenCabinet = post.Accommodation.AccommodationDetail.HasKitchenCabinet,
                HasAirConditioner = post.Accommodation.AccommodationDetail.HasAirConditioner,
                HasRefrigerator = post.Accommodation.AccommodationDetail.HasRefrigerator,
                HasWashingMachine = post.Accommodation.AccommodationDetail.HasWashingMachine,
                HasLoft = post.Accommodation.AccommodationDetail.HasLoft,
                FurnitureStatus = post.Accommodation.AccommodationDetail.FurnitureStatus,
                CreatedAt = post.Accommodation.AccommodationDetail.CreatedAt,
                UpdatedAt = post.Accommodation.AccommodationDetail.UpdatedAt,
                Address = post.Accommodation.Address ?? "",
                AccountId = post.Account.AccountId,
                AccountImg = post.Account?.UserProfile?.AvatarUrl ?? "/images/default-avatar.jpg",
                AccountName = post.Account?.UserProfile?.FirstName + " " + post.Account?.UserProfile?.LastName,
                AccountPhone = post.Account?.UserProfile?.PhoneNumber,
                Amenities = post.Accommodation.AccommodationAmenities?
                    .Where(a => a.Amenity != null)
                    .Select(a => a.Amenity.AmenityName)
                    .ToList() ?? new List<string>(),
                DistrictName = post.Accommodation.DistrictName ?? "",
                ProvinceName = post.Accommodation.ProvinceName ?? "",
                WardName = post.Accommodation.WardName ?? "",

                PackageTypeName = latestPackage?.Pricing?.PackageType?.PackageTypeName ?? "Tin thường",
                TimeUnitName = latestPackage?.Pricing?.TimeUnit?.TimeUnitName ?? "",
                TotalPrice = latestPackage?.TotalPrice ?? 0,
                StartDate = latestPackage?.StartDate,
                EndDate = latestPackage?.EndDate,
                Comments = await _postCommentService.GetCommentsByPostIdAsync(post.PostId),
            };


            ViewData["Address"] = viewModel.Address;

            return View(viewModel);
        }


        [HttpPost]
        [Route("/api/v1/add-comment")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Cookie, Roles = $"{UserRoles.User},{UserRoles.Landlord}")]
        public async Task<IActionResult> AddComment([FromBody] AddPostCommentDto dto)
        {
            try
            {
                var accountId = User.GetUserId();
                var account = await _accountService.GetAccountById(accountId.Value);

                if (accountId == null)
                    return Json(new { success = false, message = "Bạn cần đăng nhập để bình luận." });

                var newComment = new PostComment
                {
                    PostId = dto.PostId,
                    AccountId = accountId.Value,
                    Comment = dto.Content,
                    CreatedAt = DateTime.Now,
                    ParentCommentId = dto.ParentCommentId
                };

                await _postCommentService.AddCommentAsync(newComment);

                var commentDto = new
                {
                    commentId = newComment.CommentId,
                    postId = newComment.PostId,
                    parentCommentId = newComment.ParentCommentId,
                    content = newComment.Comment,
                    createdAt = newComment.CreatedAt?.ToString("yyyy-MM-dd HH:mm:ss"),
                    accountName = account?.UserProfile?.FirstName + " " + account?.UserProfile?.LastName,
                    accountAvatarUrl = account?.UserProfile?.AvatarUrl ?? "/images/default-avatar.jpg",
                };

                return Json(new { success = true, comment = commentDto });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> Search(string provinceName, string districtName, string? wardName, double? area, decimal? minMoney, decimal? maxMoney, string provinceId, string districtId, string? wardId)
        {
            //luu gia tri
            TempData["provinceName"] = provinceName;
            TempData["districtName"] = districtName;
            TempData["wardName"] = wardName;
            TempData["provinceId"] = provinceId;
            TempData["districtId"] = districtId;
            TempData["wardId"] = wardId;
            TempData["area"] = area?.ToString();
            TempData["minMoney"] = minMoney?.ToString();
            TempData["maxMoney"] = maxMoney?.ToString();

            if (!ModelState.IsValid)
                return View();

            var accommodations = await _accommodationService.GetAccommodationsBySearchDto(provinceName, districtName, wardName, area, minMoney, maxMoney);

            var viewModelList = accommodations.Select(p =>
            {
                var latestPackageDetail = p.PostPackageDetails
                    .OrderByDescending(d => d.StartDate)
                    .FirstOrDefault();

                return new AccommodationIndexViewModel
                {
                    Id = p.PostId,
                    Status = p.CurrentStatus,
                    Title = p.Title,
                    Price = p.Accommodation.Price,
                    Address = p.Accommodation.Address,
                    Area = p.Accommodation.Area,
                    BathroomCount = p.Accommodation?.AccommodationDetail?.BathroomCount,
                    BedroomCount = p.Accommodation?.AccommodationDetail?.BedroomCount,
                    ImageUrl = p.Accommodation?.AccommodationImages?.FirstOrDefault()?.ImageUrl ?? "default-image.jpg",
                    CreatedAt = p.CreatedAt,
                    DistrictName = p.Accommodation?.DistrictName ?? "",
                    ProvinceName = p.Accommodation?.ProvinceName ?? "",
                    WardName = p.Accommodation?.WardName ?? "",
                    PackageTypeName = latestPackageDetail?.Pricing?.PackageType?.PackageTypeName ?? "",
                    TimeUnitName = latestPackageDetail?.Pricing?.TimeUnit?.TimeUnitName ?? "",
                    TotalPrice = latestPackageDetail?.TotalPrice ?? 0,
                    StartDate = latestPackageDetail?.StartDate,
                    EndDate = latestPackageDetail?.EndDate
                };
            }).ToList();

            TempData["RoomListSearch"] = JsonConvert.SerializeObject(viewModelList);
            TempData["HasSearched"] = true;

            return RedirectToAction("Index", "Accommodations");
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = AuthSchemes.Cookie, Roles = $"{UserRoles.User},{UserRoles.Landlord}")]
        public async Task<IActionResult> IsFavorite(int postId)
        {
            var accountId = User.GetUserId();
            var isFavorite = await _favoriteService.IsFavorite(postId, accountId ?? 0);
            return Json(isFavorite);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = AuthSchemes.Cookie, Roles = $"{UserRoles.User},{UserRoles.Landlord}")]
        public async Task<IActionResult> AddToFavorite(int postId)
        {
            var accountId = User.GetUserId();
            if (accountId == 0)
            {
                return Unauthorized();
            }

            await _favoriteService.AddToFavorite(postId, accountId ?? 0);
            return Ok();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = AuthSchemes.Cookie, Roles = $"{UserRoles.User},{UserRoles.Landlord}")]
        public async Task<IActionResult> RemoveFromFavorite(int postId)
        {
            var accountId = User.GetUserId();

            if (accountId == 0)
            {
                return Unauthorized();
            }

            await _favoriteService.RemoveFromFavorite(postId, accountId ?? 0);
            return Ok();
        }

        [HttpGet("/bat-dau-tro-chuyen")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Cookie, Roles = $"{UserRoles.User},{UserRoles.Landlord}")]
        public async Task<IActionResult> StartConversation(int postId, int receiverId)
        {
            int senderId = User.GetUserId() ?? 0;

            var conversation = await _conversationService.AddIfNotExistsAsync(senderId, receiverId, postId);

            TempData["OpenedConversationId"] = conversation.ConversationId;

            return RedirectToAction("Index", "ChatRoom");
        }
    }
}
