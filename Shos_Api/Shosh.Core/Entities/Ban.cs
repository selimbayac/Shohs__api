using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Core.Entities
{
    public class Ban
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public DateTime BannedAt { get; set; } = DateTime.UtcNow;
        public DateTime? BanExpiresAt { get; set; } // Süresiz ban için null olabilir

        public string Reason { get; set; } = string.Empty;
        public DateTime BanCreatedAt { get; set; }
    }
}
