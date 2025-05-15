using Shosh.Core.Entities;
using Shosh.Data.IRepository;
using Shosh.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Service.Service
{
    public class BanService : IBanService
    {
        private readonly IBanRepository _banRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;

        public BanService(IBanRepository banRepository, IUserRepository userRepository, INotificationService notificationService)
        {
            _banRepository = banRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
        }

        public async Task<bool> BanUserAsync(int userId, string reason, int? durationInDays)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return false;

            var ban = new Ban
            {
                UserId = userId,
                Reason = reason,
                BanCreatedAt = DateTime.UtcNow,
                BanExpiresAt = durationInDays.HasValue ? DateTime.UtcNow.AddDays(durationInDays.Value) : null
            };

            user.IsBanned = true;
            user.Role = "Banned"; // 📌 Kullanıcının rolünü değiştiriyoruz
            await _banRepository.AddBanAsync(ban);
            await _userRepository.UpdateUserAsync(user);

            // 📌 Kullanıcıya bildirim gönder
            await _notificationService.SendNotificationAsync(userId, $"Şu sebepten ötürü banlandınız: {reason}. Süre: {durationInDays?.ToString() ?? "Süresiz"} gün.");

            return true;
        }
        public async Task<bool> UnbanUserAsync(int userId)
        {
            var ban = await _banRepository.GetBanDetailsAsync(userId);
            if (ban == null) return false; // Kullanıcı zaten banlı değilse

            await _banRepository.RemoveBanAsync(userId); // Ban kaydını veritabanından sil
            return true;
        }

        public async Task<bool> IsUserBannedAsync(int userId)
        {
            return await _banRepository.IsUserBannedAsync(userId);
        }
        public async Task<Ban?> GetBanDetailsAsync(int userId)
        {
            return await _banRepository.GetBanDetailsAsync(userId);
        }
        // 📌 **Ban süresi dolan kullanıcıları otomatik aç**
        public async Task UnbanExpiredUsersAsync()
        {
            await _banRepository.UnbanExpiredUsersAsync();
        }
        public async Task<List<Ban>> GetExpiredBansAsync()
        {
            return await _banRepository.GetExpiredBansAsync();
        }
    }
}