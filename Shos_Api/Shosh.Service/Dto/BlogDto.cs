using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Service.Dto
{
    public class BlogDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int LikeCount { get; set; } // Beğeni Sayısı
        public int DislikeCount { get; set; } // Dislike Sayısı
        public int CommentCount { get; set; } // Yorum Sayısı

        // Blog yazarı bilgisi
        public int UserId { get; set; }
        public string UserNickname { get; set; } = string.Empty;
    }
}
