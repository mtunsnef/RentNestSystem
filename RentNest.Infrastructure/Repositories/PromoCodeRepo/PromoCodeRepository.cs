using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.PromoCodeRepo
{
    public class PromoCodeRepository : RepositoryBase<PromoCode>, IPromoCodeRepository
    {
        public PromoCodeRepository(Db21027Context context) : base(context)
        {
        }

        public async Task<List<PromoCode>> GetPromotionsAsync(string keyword = "")
        {
            return await _dbSet
                .Where(p => (p.Code.Contains(keyword))
                            && p.StartDate <= DateTime.Now
                            && p.EndDate >= DateTime.Now)
                .ToListAsync();
        }
    }
}
