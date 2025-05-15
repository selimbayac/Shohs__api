using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shosh.Core.Entities
{
    public class Entry
    {
        [Key]
        public int Id { get; set; }

        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // **Entry'yi yazan kullanıcı**
        public int UserId { get; set; }
        public User User { get; set; }

        // **Entry'nin ait olduğu başlık**
        public int TopicId { get; set; }
        public Topic? Topic { get; set; }

        // **Entry'ye gelen beğeniler**
        [JsonIgnore]
        public ICollection<Like> Likes { get; set; } = new List<Like>();

        // **Entry'ye gelen yorumlar**
        [JsonIgnore]
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        // **LikeCount ve DislikeCount öz niteliklerini buraya ekliyoruz**
        [NotMapped]  // Bu alanlar veritabanında saklanmayacak
        public int LikeCount { get; set; }
        [NotMapped]
        public int DislikeCount { get; set; }
    }
}
