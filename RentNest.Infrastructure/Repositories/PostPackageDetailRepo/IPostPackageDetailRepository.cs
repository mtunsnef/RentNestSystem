using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.PostPackageDetailRepo
{
    public interface IPostPackageDetailRepository : IRepositoryBase<PostPackageDetail>
    {
        Task<PostPackageDetail?> GetByPostIdAsync(int postId);
    }
}
