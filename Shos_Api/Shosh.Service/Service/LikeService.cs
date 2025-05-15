using Microsoft.EntityFrameworkCore;
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
    public class LikeService : ILikeService
    {
        private readonly ILikeRepository _likeRepository;

        public LikeService(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        public async Task<bool> LikeEntryAsync(int userId, int entryId, bool isLike)
        {
            return await _likeRepository.LikeEntryAsync(userId, entryId, isLike);
        }

        public async Task<bool> LikeCommentAsync(int userId, int commentId, bool isLike)
        {
            return await _likeRepository.LikeCommentAsync(userId, commentId, isLike);
        }

        public async Task<bool> RemoveLikeAsync(int userId, int entryId)
        {
            var existingLike = await _likeRepository.GetUserLikeForEntryAsync(userId, entryId);
            if (existingLike == null)
                return false;

            return await _likeRepository.RemoveLikeAsync(userId, entryId);
        }

        public async Task<int> GetLikeCountForEntryAsync(int entryId, bool isLike)
        {
            return await _likeRepository.GetLikeCountForEntryAsync(entryId, isLike);
        }

        public async Task<int> GetLikeCountForCommentAsync(int commentId, bool isLike)
        {
            return await _likeRepository.GetLikeCountForCommentAsync(commentId, isLike);
        }

        public async Task<Like?> GetUserLikeForEntryAsync(int userId, int entryId)
        {
            return await _likeRepository.GetUserLikeForEntryAsync(userId, entryId);
        }
        public async Task<int> GetTotalLikesForUserAsync(int userId)
        {
            return await _likeRepository.GetTotalLikesForUserAsync(userId);
        }

        public async Task<int> GetTotalDislikesForUserAsync(int userId)
        {
            return await _likeRepository.GetTotalDislikesForUserAsync(userId);
        }
        public async Task<bool> LikeBlogAsync(int userId, int blogId, bool isLike)
        {
            return await _likeRepository.LikeBlogAsync(userId, blogId, isLike);
        }

        public async Task<int> GetLikeCountForBlogAsync(int blogId, bool isLike)
        {
            return await _likeRepository.GetLikeCountForBlogAsync(blogId, isLike);
        }

        public async Task<Like?> GetUserLikeForBlogAsync(int userId, int blogId)
        {
            return await _likeRepository.GetUserLikeForBlogAsync(userId, blogId);
        }


    }
}
