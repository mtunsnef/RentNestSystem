using Microsoft.EntityFrameworkCore;
using RentNest.Infrastructure.Repositories.PromoCodeRepo;
using RentNest.Infrastructure.Repositories.PromoUsageRepo;
using RentNest.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Services.PromoCodeService
{
    public class PromoCodeService : IPromoCodeService
    {
        private readonly IPromoCodeRepository _promoCodeRepository;
        private readonly IPromoUsageRepository _promoUsageRepository;
        public PromoCodeService(IPromoCodeRepository promoCodeRepository, IPromoUsageRepository promoUsageRepository)
        {
            _promoCodeRepository = promoCodeRepository;
            _promoUsageRepository = promoUsageRepository;   
        }
        public async Task<List<PromotionDto>> GetPromotionsAsync(string keyword, int accountId)
        {
            var usedPromoIds = await _promoUsageRepository.GetPromoUsagesByAccountId(accountId);
            var usedPromoIdSet = new HashSet<int>(usedPromoIds); // để check nhanh

            var allPromotions = await _promoCodeRepository.GetPromotionsAsync();

            var availablePromotions = allPromotions
                .Where(p => !usedPromoIdSet.Contains(p.PromoId))
                .Where(p => string.IsNullOrEmpty(keyword) ||
                            p.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                            p.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .Select(p => new PromotionDto
                {
                    PromoId = p.PromoId,
                    Code = p.Code,
                    Description = p.Description,
                    DiscountAmount = p.DiscountAmount,
                    DurationDays = p.DurationDays
                })
                .ToList();

            return availablePromotions;
        }


    }
}
