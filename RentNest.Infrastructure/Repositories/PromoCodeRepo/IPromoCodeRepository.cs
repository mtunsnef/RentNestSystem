using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.PromoCodeRepo
{
    public interface IPromoCodeRepository : IRepositoryBase<PromoCode>
    {
        Task<List<PromoCode>> GetPromotionsAsync(string keyword = "");
    }
}
