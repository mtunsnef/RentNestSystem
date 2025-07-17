using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.DTOs
{
    public class PromotionDto
    {
        public int PromoId { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public int? DurationDays { get; set; }
        public decimal? DiscountAmount { get; set; }
    }

}
