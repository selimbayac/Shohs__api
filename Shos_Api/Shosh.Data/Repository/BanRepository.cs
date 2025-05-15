using Microsoft.EntityFrameworkCore;
using Shosh.Core.Entities;
using Shosh.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Data.Repository
{
    public class BanRepository : IBanRepository
    {
        private readonly ApplicationDbContext _context;
      
        public BanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsUserBannedAsync(int userId)
        {
            return await _context.Bans.AnyAsync(b => b.UserId == userId &&
                (b.BanExpiresAt == null || b.BanExpiresAt > DateTime.UtcNow));
        }

        public async Task AddBanAsync(Ban ban)
        {
            await _context.Bans.AddAsync(ban);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveBanAsync(int userId)
        {
            var ban = await _context.Bans.FirstOrDefaultAsync(b => b.UserId == userId);
            if (ban != null)
            {
                _context.Bans.Remove(ban);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Ban?> GetBanDetailsAsync(int userId)
        {
            return await _context.Bans.FirstOrDefaultAsync(b => b.UserId == userId);
        }

        // 📌 **Ban süresi dolan kullanıcıları otomatik aç**
        public async Task UnbanExpiredUsersAsync()
        {
            var expiredBans = await _context.Bans.Where(b => b.BanExpiresAt <= DateTime.UtcNow).ToListAsync();
            if (expiredBans.Any())
            {
                _context.Bans.RemoveRange(expiredBans);
                await _context.SaveChangesAsync();
            }

        }
        public async Task<List<Ban>> GetExpiredBansAsync()
        {
            return await _context.Bans
                .Where(b => b.BanExpiresAt != null && b.BanExpiresAt <= DateTime.UtcNow)
                .ToListAsync();
        }
    }
}
