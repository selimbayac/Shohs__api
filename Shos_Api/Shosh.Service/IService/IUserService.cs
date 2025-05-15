using Shosh.Core.Entities;
using Shosh.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Service.IService
{
    public interface IUserService
    {
        Task<string?> RegisterAsync(string email, string password, string nickname);
        Task<User?> LoginUserAsync(string emailOrNickname, string password);
        Task<bool> UpdateUserAsync(int userId, string? email, string? nickname, string? bio);
        Task<User?> GetUserByIdAsync(int userId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByNicknameAsync(string nickname);
        Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
        Task<bool> SendEmailVerificationAsync(string email);
        Task<bool> VerifyEmailAsync(string email, string verificationCode);
        Task<bool> SendPasswordResetCodeAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string verificationCode, string newPassword);
        string GenerateJwtToken(User user);
        Task<bool> SetUserRoleAsync(int userId, string role);
        Task<bool> BanUserAsync(int userId, int? days, string reason);
        Task AutoUnbanUsersAsync();
        Task<bool> UnbanUserAsync(int userId);
        Task<List<User>> GetAllUsersAsync();
        Task<UserProfileDto> GetUserProfileAsync(int userId);
    }
}
