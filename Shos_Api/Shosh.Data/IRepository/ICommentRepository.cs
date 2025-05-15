using Shosh.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Data.IRepository
{
    public interface ICommentRepository
    {
        Task AddCommentAsync(Comment comment);
        Task<Comment?> GetCommentByIdAsync(int commentId);
        Task<List<Comment>> GetCommentsByEntryIdAsync(int entryId);
        Task<bool> DeleteCommentAsync(int commentId);
    }
}
