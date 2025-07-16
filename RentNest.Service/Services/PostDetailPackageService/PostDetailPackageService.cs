using RentNest.Core.Domains;
using RentNest.Infrastructure.Repositories.PostPackageDetailRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Services.PostDetailPackageService
{
    public class PostDetailPackageService : IPostDetailPackageService
    {
        private readonly IPostPackageDetailRepository _postPackageDetailRepository;

        public PostDetailPackageService(IPostPackageDetailRepository postPackageDetailRepository)
        {
            _postPackageDetailRepository = postPackageDetailRepository;
        }

        public async Task<PostPackageDetail?> GetByPostIdAsync(int postId)
        {
            return await _postPackageDetailRepository.GetByPostIdAsync(postId);
        }

        public async Task UpdateAsync(PostPackageDetail postPackageDetail)
        {
            await _postPackageDetailRepository.UpdateAsync(postPackageDetail);
        }
    }
}
