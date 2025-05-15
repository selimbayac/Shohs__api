using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Application.Dtos
{
    public class ComplaintDto
    {
        public int Id { get; set; }
        public string Complainant { get; set; } // Şikayeti yapan kullanıcı adı
        public string TargetUser { get; set; } // Şikayet edilen kullanıcı adı
        public string Reason { get; set; } // Şikayet sebebi
        public string Content { get; set; } // Şikayet içeriği
        public DateTime CreatedAt { get; set; } // Şikayet tarihi
    }
}
