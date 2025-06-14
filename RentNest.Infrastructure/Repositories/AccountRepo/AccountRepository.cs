using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.AccountRepo
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(Db21027Context context) : base(context) { }

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _dbSet.AnyAsync(a => a.Email == email);
        }

        public async Task<bool> CheckUsernameExistsAsync(string username)
        {
            return await _dbSet.AnyAsync(a => a.Username == username);
        }
        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            return await _dbSet.Include(u => u.UserProfile).FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<Account?> GetAccountByEmailOrUsernameAsync(string input)
        {
            return await _context.Accounts
                 .Include(a => a.UserProfile)
                .FirstOrDefaultAsync(a => a.Email.ToLower() == input.ToLower() || a.Username.ToLower() == input.ToLower());
        }
    }
}
