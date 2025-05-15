using Shosh.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Data.IRepository
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int userId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByNicknameAsync(string nickname);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);  // 📌 **GÜNCELLENDİ**
        Task DeleteUserAsync(User user);

        // 📌 **E-posta Doğrulama & Şifre Sıfırlama**
        Task<bool> UpdateEmailVerificationAsync(string email, string verificationCode, DateTime expiresAt);
        Task<bool> UpdatePasswordResetCodeAsync(string email, string resetCode, DateTime expiresAt);

        // 📌 **Kullanıcı Mevcut mu?**
        Task<bool> UserExistsByEmailAsync(string email);
        Task<bool> UserExistsByNicknameAsync(string nickname);

        Task<List<User>> GetAllUsersAsync();
    }
}
