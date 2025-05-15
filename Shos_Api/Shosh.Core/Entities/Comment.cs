using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shosh.Core.Entities
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // **Yorumu yazan kullanıcı**
        public int UserId { get; set; }
        public User User { get; set; }

        // **Yorumun bağlı olduğu Entry**
        public int EntryId { get; set; }
        public Entry Entry { get; set; }

        // **Yoruma gelen beğeniler**
        [JsonIgnore]
        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}