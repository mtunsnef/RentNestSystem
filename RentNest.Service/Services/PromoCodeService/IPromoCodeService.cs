using RentNest.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Services.PromoCodeService
{
    public interface IPromoCodeService
    {
        Task<List<PromotionDto>> GetPromotionsAsync(string keyword, int accountId);
    }
}
