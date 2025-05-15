using Shosh.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Data.IRepository
{
    public interface IBanRepository
    {
        Task<bool> IsUserBannedAsync(int userId);
        Task AddBanAsync(Ban ban);
        Task RemoveBanAsync(int userId);
        Task<Ban?> GetBanDetailsAsync(int userId);
        Task UnbanExpiredUsersAsync();  // 📌 Eklenen yeni metot
        Task<List<Ban>> GetExpiredBansAsync();
    }
}
