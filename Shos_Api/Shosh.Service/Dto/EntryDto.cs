using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Service.Dto
{
    public class EntryDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string TopicTitle { get; set; } 
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = "Bilinmeyen Kullanıcı"; // Eğer kullanıcı null gelirse default değer
        public int LikeCount { get; set; } = 0;
        public int DislikeCount { get; set; } = 0;
    }
}
