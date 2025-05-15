using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Core.Entities
{
    public class Like
    {
        [Key]
        public int Id { get; set; }

        // 📌 **Beğeniyi yapan kullanıcı**
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        // 📌 **Entry beğenisi**
        public int? EntryId { get; set; }
        [ForeignKey("EntryId")]
        public Entry? Entry { get; set; }

        // 📌 **Comment beğenisi**
        public int? CommentId { get; set; }
        [ForeignKey("CommentId")]
        public Comment? Comment { get; set; }

        public int? BlogId { get; set; }
        [ForeignKey("BlogId")]
        public Blog? Blog { get; set; }
        // 📌 **Beğeni mi? Dislike mı?**
        public bool IsLike { get; set; } // True = Like, False = Dislike
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
    
}
