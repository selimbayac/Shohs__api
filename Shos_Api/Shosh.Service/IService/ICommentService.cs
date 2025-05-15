using Shosh.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Service.IService
{
    public interface ICommentService
    {
        Task<bool> AddCommentAsync(int userId, int entryId, string content);
        Task<List<Comment>> GetCommentsByEntryIdAsync(int entryId);
        Task<bool> DeleteCommentAsync(int commentId, int userId, string userRole);
    }
}
