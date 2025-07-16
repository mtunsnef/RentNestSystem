using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Services.PostDetailPackageService
{
    public interface IPostDetailPackageService
    {
        Task<PostPackageDetail?> GetByPostIdAsync(int postId);
        Task UpdateAsync(PostPackageDetail postPackageDetail);

    }
}
