using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Core.Entities
{
    public enum UserRole
    {
        User,       // Normal Kullanıcı
        Moderator,  // Yorum & Entry silebilir, ban atabilir
        Admin       // Kullanıcı silebilir
    }
}
