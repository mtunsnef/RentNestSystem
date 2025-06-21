using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.UserProfileRepo
{
    public interface IUserProfileRepository : IRepositoryBase<UserProfile>
    {
        Task<UserProfile?> GetProfileByAccountIdAsync(int accountId);
        Task<UserProfile?> FindExistingUserProfileAsync(UserProfile updatedProfile);
    }
}
