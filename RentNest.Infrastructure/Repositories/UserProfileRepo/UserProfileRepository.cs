using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
using RentNest.Infrastructure.Repositories.PaymentRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.UserProfileRepo
{
    public class UserProfileRepository : RepositoryBase<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(Db21027Context context) : base(context) { }

        public async Task<UserProfile?> GetProfileByAccountIdAsync(int accountId)
        {
            return await _dbSet
                .Include(p => p.Account)
                .FirstOrDefaultAsync(p => p.AccountId == accountId);
        }

        public async Task<UserProfile?> FindExistingUserProfileAsync(UserProfile updatedProfile)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.AccountId == updatedProfile.AccountId);
        }
    }
}
