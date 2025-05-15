using Shosh.Application.Dtos;
using Shosh.Core.Entities;

namespace Shosh.Application.Dtos
{
    public class LikeRequestDto
    {

        public int TargetId { get; set; } // Entry veya Comment ID'si
        public LikeType Type { get; set; } // Entry mi? Comment mi?
        public bool IsLike { get; set; } // True = Beğeni, False = Dislike
    }
}
