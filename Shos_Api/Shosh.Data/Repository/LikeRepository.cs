using Microsoft.EntityFrameworkCore;
using Shosh.Core.Entities;
using Shosh.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Data.Repository
{
    public class LikeRepository : ILikeRepository
    {
        private readonly ApplicationDbContext _context;

        public LikeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> LikeEntryAsync(int userId, int entryId, bool isLike)
        {
            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.EntryId == entryId);

            if (existingLike != null)
            {
                if (existingLike.IsLike == isLike)
                {
                    _context.Likes.Remove(existingLike); // Aynı butona basıldıysa kaldır
                }
                else
                {
                    existingLike.IsLike = isLike; // Beğeni değiştirildi
                }
            }
            else
            {
                var like = new Like
                {
                    UserId = userId,
                    EntryId = entryId,
                    IsLike = isLike
                };
                await _context.Likes.AddAsync(like);
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> LikeCommentAsync(int userId, int commentId, bool isLike)
        {
            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.CommentId == commentId);

            if (existingLike != null)
            {
                if (existingLike.IsLike == isLike)
                {
                    _context.Likes.Remove(existingLike); // Aynı butona basıldıysa kaldır
                }
                else
                {
                    existingLike.IsLike = isLike; // Beğeni değiştirildi
                }
            }
            else
            {
                var like = new Like
                {
                    UserId = userId,
                    CommentId = commentId,
                    IsLike = isLike
                };
                await _context.Likes.AddAsync(like);
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveLikeAsync(int userId, int entryId)
        {
            var like = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.EntryId == entryId);

            if (like == null)
                return false;

            _context.Likes.Remove(like);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> GetLikeCountForEntryAsync(int entryId, bool isLike)
        {
            return await _context.Likes.CountAsync(l => l.EntryId == entryId && l.IsLike == isLike);
        }

        public async Task<int> GetLikeCountForCommentAsync(int commentId, bool isLike)
        {
            return await _context.Likes.CountAsync(l => l.CommentId == commentId && l.IsLike == isLike);
        }

        public async Task<Like?> GetUserLikeForEntryAsync(int userId, int entryId)
        {
            return await _context.Likes.FirstOrDefaultAsync(l => l.UserId == userId && l.EntryId == entryId);
        }
        public async Task<int> GetTotalLikesForUserAsync(int userId)
        {
            return await _context.Likes.CountAsync(l => l.UserId == userId && l.IsLike == true);
        }

        public async Task<int> GetTotalDislikesForUserAsync(int userId)
        {
            return await _context.Likes.CountAsync(l => l.UserId == userId && l.IsLike == false);
        }

        public async Task<bool> LikeBlogAsync(int userId, int blogId, bool isLike)
        {
            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.BlogId == blogId);

            if (existingLike != null)
            {
                if (existingLike.IsLike == isLike)
                {
                    _context.Likes.Remove(existingLike); // Aynı butona basıldıysa kaldır
                }
                else
                {
                    existingLike.IsLike = isLike; // Beğeni değiştirildi
                }
            }
            else
            {
                var like = new Like
                {
                    UserId = userId,
                    BlogId = blogId,
                    IsLike = isLike
                };
                await _context.Likes.AddAsync(like);
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> GetLikeCountForBlogAsync(int blogId, bool isLike)
        {
            return await _context.Likes.CountAsync(l => l.BlogId == blogId && l.IsLike == isLike);
        }

        public async Task<Like?> GetUserLikeForBlogAsync(int userId, int blogId)
        {
            return await _context.Likes.FirstOrDefaultAsync(l => l.UserId == userId && l.BlogId == blogId);
        }

    }
}
