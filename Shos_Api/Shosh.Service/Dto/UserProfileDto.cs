using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Service.Dto
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string? Bio { get; set; }
        public int TotalLikes { get; set; }
        public int TotalDislikes { get; set; }
    }

}
