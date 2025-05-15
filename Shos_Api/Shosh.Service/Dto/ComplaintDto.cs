using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Service.Dto
{
    public class ComplaintDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }  // Şikayet eden ID
        public int TargetUserId { get; set; } // Şikayet edilen ID
        public string Complainant { get; set; } // Şikayeti yapan kullanıcı adı
        public string TargetUser { get; set; } // Şikayet edilen kullanıcı adı
        public string Reason { get; set; } // Şikayet sebebi
        public string Content { get; set; } // Şikayet içeriği
        public DateTime CreatedAt { get; set; } // Şikayet tarihi

        // 📌 **Şikayet edilen içerik bilgisi**
        public int? EntryId { get; set; }  // Entry ID
        public string? EntryContent { get; set; } // Entry içeriği

        public int? CommentId { get; set; } // Yorum ID
        public string? CommentContent { get; set; } // Yorum içeriği

        public int? BlogId { get; set; } // Blog ID
        public string? BlogTitle { get; set; } // Blog başlığı
    }

}
