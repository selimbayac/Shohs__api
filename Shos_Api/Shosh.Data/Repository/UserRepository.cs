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
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync(); // 📌 Tüm kullanıcıları getir
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    Console.WriteLine($"❌ HATA: Veritabanında kullanıcı bulunamadı! (UserID: {userId})");
                }
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🚨 GetUserByIdAsync HATA: {ex.Message}");
                return null;
            }
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByNicknameAsync(string nickname)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Nickname == nickname);
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)  // 📌 **GÜNCELLENDİ**
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateEmailVerificationAsync(string email, string verificationCode, DateTime expiresAt)
        {
            var user = await GetUserByEmailAsync(email);
            if (user == null) return false;

            user.EmailVerificationCode = verificationCode;
            user.EmailVerificationExpiresAt = expiresAt;

            await UpdateUserAsync(user);  // 📌 **DÜZELTİLDİ**
            return true;
        }

        public async Task<bool> UpdatePasswordResetCodeAsync(string email, string resetCode, DateTime expiresAt)
        {
            var user = await GetUserByEmailAsync(email);
            if (user == null) return false;

            user.EmailVerificationCode = resetCode;
            user.EmailVerificationExpiresAt = expiresAt;

            await UpdateUserAsync(user);  // 📌 **DÜZELTİLDİ**
            return true;
        }

        public async Task<bool> UserExistsByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> UserExistsByNicknameAsync(string nickname)
        {
            return await _context.Users.AnyAsync(u => u.Nickname == nickname);
        }
    }
}
