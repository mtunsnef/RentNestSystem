using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.AccountRepo
{
    public interface IAccountRepository : IRepositoryBase<Account>
    {
        Task<Account?> GetAccountByEmailAsync(string email);
        Task<bool> CheckEmailExistsAsync(string email);
        Task<bool> CheckUsernameExistsAsync(string username);
        Task<Account?> GetAccountByEmailOrUsernameAsync(string input);
    }
}
