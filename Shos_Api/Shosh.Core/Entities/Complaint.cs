using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Core.Entities
{
    public class Complaint
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("TargetUser")]
        public int TargetUserId { get; set; }
        public User TargetUser { get; set; }

        public string Content { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; }

        // Şikayet edilen içeriğe referanslar
        public int? EntryId { get; set; }
        public Entry? Entry { get; set; }

        public int? CommentId { get; set; }
        public Comment? Comment { get; set; }

        public int? BlogId { get; set; }
        public Blog? Blog { get; set; }
    }
}
