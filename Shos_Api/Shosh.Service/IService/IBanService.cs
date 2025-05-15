using Shosh.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Service.IService
{
    public interface IBanService
    {
        Task<bool> BanUserAsync(int userId, string reason, int? durationInDays);
        Task<bool> UnbanUserAsync(int userId);
        Task<bool> IsUserBannedAsync(int userId);
        Task<Ban?> GetBanDetailsAsync(int userId);
        Task UnbanExpiredUsersAsync();  // 📌 Yeni metot eklendi
        Task<List<Ban>> GetExpiredBansAsync();
    }
}
