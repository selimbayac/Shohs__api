using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Core.Entities
{
    public class UserFollow
    {
        [Key]
        public int Id { get; set; }

        // 📌 Takip eden kullanıcı
        public int FollowerId { get; set; }
        [ForeignKey("FollowerId")]
        public User Follower { get; set; }

        // 📌 Takip edilen kullanıcı
        public int FollowingId { get; set; }
        [ForeignKey("FollowingId")]
        public User Following { get; set; }

        // 📌 Takip tarihi
        public DateTime FollowedAt { get; set; } = DateTime.UtcNow;
    }
}
