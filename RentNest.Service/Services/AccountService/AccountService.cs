using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
using RentNest.Core.Utils;
using RentNest.Infrastructure.Repositories.AccountRepo;
using RentNest.Infrastructure.Repositories.UserProfileRepo;
using RentNest.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        public AccountService(IAccountRepository accountRepository, IUserProfileRepository userProfileRepository)
        {
            _accountRepository = accountRepository;
            _userProfileRepository = userProfileRepository;
        }

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _accountRepository.CheckEmailExistsAsync(email);
        }

        public async Task<bool> CheckUsernameExistsAsync(string username)
        {
            return await _accountRepository.CheckUsernameExistsAsync(username);
        }

        public async Task<Account> CreateExternalAccountAsync(ExternalAccountRegisterDto dto)
        {
            var account = new Account
            {
                Email = dto.Email,
                AuthProvider = dto.AuthProvider.ToLower(),
                AuthProviderId = dto.AuthProviderId,
                Role = dto.Role,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _accountRepository.AddAsync(account);

                var userProfile = new UserProfile
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Address = dto.Address,
                    PhoneNumber = dto.PhoneNumber,
                    CreatedAt = DateTime.UtcNow,
                    AccountId = account.AccountId
                };

                await _userProfileRepository.AddAsync(userProfile);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tạo tài khoản: " + ex.Message);
            }
            return account;
        }

        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            return await _accountRepository.GetAccountByEmailAsync(email);
        }

        public async Task<Account?> GetAccountByEmailOrUsernameAsync(string input)
        {
            return await _accountRepository.GetAccountByEmailOrUsernameAsync(input);
        }

        public async Task<Account?> GetAccountById(int accountId)
        {
            return await _accountRepository.GetByIdAsync(accountId);
        }

        public async Task<UserProfile?> GetProfileAsync(int accountId)
        {
            return await _userProfileRepository.GetProfileByAccountIdAsync(accountId);
        }

        public async Task<bool> Login(AccountLoginDto accountDto)
        {
            try
            {
                var user = await _accountRepository.GetAccountByEmailOrUsernameAsync(accountDto.EmailOrUsername);

                if (user == null || !PasswordHelper.VerifyPassword(accountDto.Password,
                                                                   user.Password ?? throw new Exception("Password null")))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi đăng nhập: " + ex.Message);
            }
        }

        public async Task<bool> RegisterAccountAsync(AccountRegisterDto model)
        {
            var account = new Account
            {
                Username = model.Username,
                Email = model.Email,
                Password = PasswordHelper.HashPassword(model.Password),
                Role = model.Role,
                IsActive = "A",
                AuthProvider = "local",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _accountRepository.AddAsync(account);

            var profile = new UserProfile
            {
                AccountId = account.AccountId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _userProfileRepository.AddAsync(profile);
            return true;
        }

        public async Task SetUserOnlineAsync(int userId, bool isOnline)
        {
            var user = await _accountRepository.GetByIdAsync(userId);
            if (user != null)
            {
                user.IsOnline = isOnline;
                await _accountRepository.UpdateAsync(user);
            }
        }

        public async Task Update(Account account)
        {
            await _accountRepository.UpdateAsync(account);
        }

        public async Task UpdateLastActiveAsync(int userId)
        {
            var user = await _accountRepository.GetByIdAsync(userId);
            if (user != null)
            {
                user.LastActiveAt = DateTime.UtcNow.AddHours(7);
                await _accountRepository.UpdateAsync(user);
            }
        }

        public async Task UpdateProfileAsync(UserProfile updatedProfile)
        {
            var existingProfile = await _userProfileRepository.FindExistingUserProfileAsync(updatedProfile);

            if (existingProfile == null)
            {
                throw new Exception("Profile not found.");
            }

            existingProfile.FirstName = updatedProfile.FirstName;
            existingProfile.LastName = updatedProfile.LastName;
            existingProfile.Gender = updatedProfile.Gender;
            existingProfile.DateOfBirth = updatedProfile.DateOfBirth;
            existingProfile.Address = updatedProfile.Address;
            existingProfile.Occupation = updatedProfile.Occupation;
            existingProfile.PhoneNumber = updatedProfile.PhoneNumber;
            existingProfile.UpdatedAt = DateTime.Now;

            if (!string.IsNullOrEmpty(updatedProfile.AvatarUrl))
            {
                existingProfile.AvatarUrl = updatedProfile.AvatarUrl;
            }

            existingProfile.UpdatedAt = DateTime.Now;

            await _userProfileRepository.UpdateAsync(existingProfile);
        }
        private async Task UpdateAvatarAsync(UserProfile profile, string avatarUrl)
        {
            profile.AvatarUrl = avatarUrl;
            await UpdateProfileAsync(profile);
        }
        public async Task<(bool Success, string Message)> UploadAvatarAsync(int accountId, IFormFile avatar, string webRootPath)
        {
            if (avatar == null || avatar.Length == 0)
                return (false, "Vui lòng chọn một ảnh hợp lệ.");

            var profile = await _userProfileRepository.GetProfileByAccountIdAsync(accountId);
            if (profile == null)
                return (false, "Không tìm thấy hồ sơ người dùng.");

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(avatar.FileName)}";
            var filePath = Path.Combine(webRootPath, "images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await avatar.CopyToAsync(stream);
            }

            await UpdateAvatarAsync(profile, $"/uploads/{fileName}");

            return (true, "Cập nhật ảnh đại diện thành công!");
        }
    }
}
