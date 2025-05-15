using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Core.Entities
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        public string Message { get; set; } // Bildirim içeriği

        public bool IsRead { get; set; } = false; // Okundu mu?

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Oluşturulma zamanı
    }
}
