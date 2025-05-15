using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Core.Entities
{
    public class Blog
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Blog yazarı
        public User User { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Comment> Comments { get; set; } = new List<Comment>(); // Blog yorumları
        public ICollection<Like> Likes { get; set; } = new List<Like>(); // Blog beğenileri
    }
}
