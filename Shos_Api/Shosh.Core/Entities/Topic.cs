using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Core.Entities
{
    public class Topic
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        public int CreatedByUserId { get; set; }
        public bool IsAnonymous { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual User CreatedByUser { get; set; }
        public virtual List<Entry> Entries { get; set; } = new();
    }
}
