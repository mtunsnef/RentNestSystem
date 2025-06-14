using Microsoft.AspNetCore.Http;
using RentNest.Core.Domains;
using RentNest.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Services.AccountService
{
    public interface IAccountService
    {
        Task<bool> Login(AccountLoginDto accountDto);
        Task<Account?> GetAccountByEmailAsync(string email);
        Task Update(Account account);
        Task<UserProfile?> GetProfileAsync(int accountId);
        Task UpdateProfileAsync(UserProfile profile);
        Task<(bool Success, string Message)> UploadAvatarAsync(int accountId, IFormFile avatar, string webRootPath);
        Task<Account> CreateExternalAccountAsync(ExternalAccountRegisterDto dto);
        Task<bool> RegisterAccountAsync(AccountRegisterDto model);
        Task SetUserOnlineAsync(int userId, bool isOnline);
        Task UpdateLastActiveAsync(int userId);
        Task<Account?> GetAccountById(int accountId);
        Task<bool> CheckEmailExistsAsync(string email);
        Task<bool> CheckUsernameExistsAsync(string username);
        Task<Account?> GetAccountByEmailOrUsernameAsync(string input);
    }
}
