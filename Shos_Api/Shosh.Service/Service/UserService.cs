using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shosh.Core.Entities;
using Shosh.Data.IRepository;
using Shosh.Data;
using Shosh.Data.Repository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Shosh.Service.IService;
using Shosh.Service.Dto;

namespace Shosh.Service.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IBanService _banService;
        private readonly INotificationService _notificationService;
        private readonly ILikeService _likeService;

        public UserService(IUserRepository userRepository, IConfiguration configuration, IBanService banService, INotificationService notificationService, ILikeService likeService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _banService = banService;
            _notificationService = notificationService;
            _likeService = likeService;
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }
        // 📌 **Kullanıcı Kaydı**
        public async Task<string?> RegisterAsync(string email, string password, string nickname)
        {
            if (await _userRepository.UserExistsByEmailAsync(email))
                return "Bu e-posta zaten kullanımda!";

            if (await _userRepository.UserExistsByNicknameAsync(nickname))
                return "Bu kullanıcı adı zaten kullanımda!";

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var newUser = new User
            {
                Email = email,
                Nickname = nickname,
                PasswordHash = hashedPassword,
                CreatedAt = DateTime.UtcNow,
                EmailVerificationCode = GenerateVerificationCode(),
                IsEmailVerified = false,
                Role = "Admin"
            };

            await _userRepository.AddUserAsync(newUser);
            return null;  // Başarılı ise null dön
        }


        public async Task<User?> LoginUserAsync(string emailOrNickname, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(emailOrNickname)
                        ?? await _userRepository.GetUserByNicknameAsync(emailOrNickname);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            // 📌 Kullanıcı banlı mı? Eğer öyleyse yine giriş yapsın AMA yazı yazamasın
            if (user.IsBanned)
            {
                var banDetails = await _banService.GetBanDetailsAsync(user.Id);
                string banMessage = $"Banlı olduğunuz için içerik ekleyemezsiniz. Sebep: {banDetails?.Reason}. Süre: {banDetails?.BanExpiresAt?.ToString("dd.MM.yyyy HH:mm") ?? "Süresiz"}";

                // 🚀 Kullanıcıya bildirim gönderelim
                await _notificationService.SendNotificationAsync(user.Id, banMessage);
            }

            return user; // ❗️ Artık token alabilir ama içerik ekleyemez
        }

        public async Task<bool> UpdateUserAsync(int userId, string? email, string? nickname, string? bio)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return false; // ❌ Kullanıcı bulunamazsa işlem iptal

            bool isUpdated = false; // 📌 Güncelleme yapıldı mı kontrolü

            if (!string.IsNullOrWhiteSpace(nickname) && user.Nickname != nickname)
            {
                if (await _userRepository.UserExistsByNicknameAsync(nickname))
                    return false; // ❌ Aynı Nickname başka kullanıcıda varsa iptal et

                user.Nickname = nickname;
                isUpdated = true;
            }

            if (!string.IsNullOrWhiteSpace(email) && user.Email != email)
            {
                if (await _userRepository.UserExistsByEmailAsync(email))
                    return false; // ❌ Aynı Email başka kullanıcıda varsa iptal et

                user.Email = email;
                isUpdated = true;
            }

            if (!string.IsNullOrWhiteSpace(bio) && user.Bio != bio)
            {
                user.Bio = bio;
                isUpdated = true;
            }

            if (!isUpdated) return false; // ❌ Hiçbir değişiklik yapılmadıysa güncelleme yapma

            await _userRepository.UpdateUserAsync(user);
            return true;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task<User?> GetUserByNicknameAsync(string nickname)
        {
            return await _userRepository.GetUserByNicknameAsync(nickname);
        }

        public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || !BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash))
                return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _userRepository.UpdateUserAsync(user);  // 📌 **HATA DÜZELTİLDİ!**
            return true;
        }

        public async Task<bool> SendEmailVerificationAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null) return false;

            string verificationCode = GenerateVerificationCode();
            DateTime expiresAt = DateTime.UtcNow.AddMinutes(10);

            return await _userRepository.UpdateEmailVerificationAsync(email, verificationCode, expiresAt);
        }

        public async Task<bool> VerifyEmailAsync(string email, string verificationCode)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null || user.EmailVerificationCode != verificationCode || user.EmailVerificationExpiresAt < DateTime.UtcNow)
                return false;

            user.IsEmailVerified = true;
            await _userRepository.UpdateUserAsync(user);  // 📌 **HATA DÜZELTİLDİ!**
            return true;
        }

        public async Task<bool> SendPasswordResetCodeAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null) return false;

            string resetCode = GenerateVerificationCode();
            DateTime expiresAt = DateTime.UtcNow.AddMinutes(10);

            return await _userRepository.UpdatePasswordResetCodeAsync(email, resetCode, expiresAt);
        }

        public async Task<bool> ResetPasswordAsync(string email, string verificationCode, string newPassword)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null || user.EmailVerificationCode != verificationCode || user.EmailVerificationExpiresAt < DateTime.UtcNow)
                return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _userRepository.UpdateUserAsync(user);  // 📌 **HATA DÜZELTİLDİ!**
            return true;
        }

        public string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role) // 📌 Kullanıcı rolü token içine eklendi
    };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateVerificationCode()
        {
            return new Random().Next(100000, 999999).ToString();
        }
        public async Task<bool> SetUserRoleAsync(int userId, string role)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return false;

            user.Role = role;
            await _userRepository.UpdateUserAsync(user);
            return true;
        }

        public async Task<bool> BanUserAsync(int userId, int? days, string reason)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return false;

            user.IsBanned = true;
            user.BanReason = reason;
            user.BanExpirationDate = days.HasValue ? DateTime.UtcNow.AddDays(days.Value) : null;
            user.Role = "Banned";

            await _userRepository.UpdateUserAsync(user);

            // ✅ `Ban` nesnesi oluşturulup `IBanService.BanUserAsync` metoduna parametre olarak gönderilmeli
            await _banService.BanUserAsync(userId, reason, days);

            // 🚀 Kullanıcıya bildirim gönder
            if (_notificationService != null)
            {
                await _notificationService.SendNotificationAsync(user.Id,
                    $"Şu sebepten ötürü banlandınız: {reason}. Süre: {days?.ToString() ?? "Süresiz"}");
            }

            return true;
        }

        // 📌 **Ban süresi dolanları otomatik aç**
        public async Task AutoUnbanUsersAsync()
        {
            await _banService.UnbanExpiredUsersAsync();  // ✅ Hata giderildi, artık IBanService kullanılıyor!
        }
        public async Task<bool> UnbanUserAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || !user.IsBanned) return false;  // Kullanıcı yoksa veya zaten banlı değilse

            user.IsBanned = false;
            user.BanReason = null;
            user.BanExpirationDate = null;
            user.Role = "User"; // Kullanıcıyı normal kullanıcı rolüne geri getir

            await _userRepository.UpdateUserAsync(user);

            // 📌 `Ban` kaydını veritabanından kaldır
            await _banService.UnbanUserAsync(userId);

            // 🚀 Kullanıcıya bildirim gönder
            if (_notificationService != null)
            {
                await _notificationService.SendNotificationAsync(user.Id, "Banınız kaldırıldı! Artık platformu kullanabilirsiniz.");
            }

            return true;
        }

        public async Task<UserProfileDto> GetUserProfileAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return null;

            var totalLikes = await _likeService.GetTotalLikesForUserAsync(userId);
            var totalDislikes = await _likeService.GetTotalDislikesForUserAsync(userId);

            var userProfile = new UserProfileDto
            {
                Id = user.Id,
                Nickname = user.Nickname,
                Email = user.Email,
                Bio = user.Bio,
                TotalLikes = totalLikes,
                TotalDislikes = totalDislikes
            };

            return userProfile;
        }
    }
}
