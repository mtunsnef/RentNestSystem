using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Services.AccommodationTypeService
{
    public interface IAccommodationTypeService
    {
        Task<IEnumerable<AccommodationType>> GetAllAsync();
    }
}
