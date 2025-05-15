using Shosh.Service.Dto;

namespace Shosh.API.ViewModel
{
    public class UserProfileDto
    {
        public int Id { get; set; } // Kullanıcının ID'si
        public string Nickname { get; set; } = string.Empty; // Kullanıcı adı
        public string Email { get; set; } = string.Empty; // E-posta adresi
        public string? Bio { get; set; } // Kullanıcının biyografi bilgisi
        public DateTime CreatedAt { get; set; } // Hesap oluşturulma tarihi
        public int FollowerCount { get; set; } // Kullanıcının takipçi sayısı
        public int FollowingCount { get; set; } // Kullanıcının takip ettiği kişi sayısı
        public int TotalLikes { get; set; } // Kullanıcının aldığı toplam beğeni sayısı
        public int TotalDislikes { get; set; } // Kullanıcının aldığı toplam dislike sayısı
       // public bool IsPrivate { get; set; } // Profilin gizli olup olmadığı // göte geldi kaldırdım

        // Ekstra bilgiler (isteğe bağlı)
        public bool IsEmailVerified { get; set; } // Kullanıcının e-posta adresinin doğrulanmış olup olmadığı
        public bool IsBanned { get; set; } // Kullanıcının yasaklanmış olup olmadığı
        public DateTime? BanExpirationDate { get; set; } // Yasak süresi varsa, bitiş tarihi


        public List<TopicDto> Topics { get; set; } // Kullanıcının yazdığı başlıklar
        public List<EntryDto> Entries { get; set; } // Kullanıcının yazdığı entry'ler
    }
}
