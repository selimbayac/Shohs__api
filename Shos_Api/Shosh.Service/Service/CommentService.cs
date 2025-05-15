using Shosh.Core.Entities;
using Shosh.Data.IRepository;
using Shosh.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Service.Service
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IEntryRepository _entryRepository;

        public CommentService(ICommentRepository commentRepository, IEntryRepository entryRepository)
        {
            _commentRepository = commentRepository;
            _entryRepository = entryRepository;
        }

        public async Task<bool> AddCommentAsync(int userId, int entryId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return false; // Boş yorum olamaz

            var entry = await _entryRepository.GetEntryByIdAsync(entryId);
            if (entry == null) return false; // Entry yoksa yorum ekleme

            var comment = new Comment
            {
                EntryId = entryId,
                UserId = userId,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };

            await _commentRepository.AddCommentAsync(comment);
            return true;
        }

        public async Task<List<Comment>> GetCommentsByEntryIdAsync(int entryId)
        {
            return await _commentRepository.GetCommentsByEntryIdAsync(entryId);
        }

        public async Task<bool> DeleteCommentAsync(int commentId, int userId, string userRole)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(commentId);
            if (comment == null) return false;

            // Eğer giriş yapan kullanıcı, yorum sahibiyse veya admin/moderatörse silebilir
            if (comment.UserId != userId && userRole != "Admin" && userRole != "Moderator")
                return false;

            return await _commentRepository.DeleteCommentAsync(commentId);
        }
    }
}
