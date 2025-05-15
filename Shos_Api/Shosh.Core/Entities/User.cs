using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Core.Entities
{
    public class User : IdentityUser<int>
    {
        [Required]
        [MaxLength(30)]
        public string Nickname { get; set; } = string.Empty;

        // 📌 **Hesap oluşturulma tarihi**
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 📌 **E-posta doğrulama kodu (6 Haneli)**
        public string? EmailVerificationCode { get; set; }

        // 📌 **E-posta doğrulama kodunun geçerlilik süresi**
        public DateTime? EmailVerificationExpiresAt { get; set; } // 📌 5 dakika geçerli olacak

        // 📌 **E-posta doğrulandı mı?**
        public bool IsEmailVerified { get; set; } = false;

        // 📌 **Kullanıcının kendisini tanıttığı biyografi alanı**
        [MaxLength(200)]
        public string Bio { get; set; } = string.Empty;

        // 📌 **Kullanıcı rolleri (Admin, Moderator, User)**
        public string Role { get; set; } = UserRole.User.ToString();

        // 📌 **Takipçiler & Takip Edilenler (Takip sistemi için)**
        public ICollection<UserFollow> Followers { get; set; } = new List<UserFollow>();
        public ICollection<UserFollow> Following { get; set; } = new List<UserFollow>();

        // 📌 **Kullanıcının yazdığı entry'ler**
        public ICollection<Entry> Entries { get; set; } = new List<Entry>();

        // 📌 **Kullanıcının yazdığı yorumlar**
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        // 📌 **Kullanıcının beğendiği entry ve yorumlar**
        public ICollection<Like> Likes { get; set; } = new List<Like>();


        public bool IsBanned { get; set; } = false;  // 🚀 Kullanıcı Banlı mı?
        public DateTime? BanExpirationDate { get; set; }  // 🚀 Ban süresi varsa burada tutulur
        public string? BanReason { get; set; }  // 🚀 Ban sebebi
    }

    

}