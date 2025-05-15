using System.ComponentModel.DataAnnotations;

namespace Shosh.Application.Dtos
{
    public class EntryLikeDto
    {
        [Required]
        public int EntryId { get; set; } // Entry ID

        public bool? IsLike { get; set; }
    }
}
