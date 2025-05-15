using Shosh.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Service.IService
{
    public interface ILikeService
    {
        Task<bool> LikeEntryAsync(int userId, int entryId, bool isLike);
        Task<bool> LikeCommentAsync(int userId, int commentId, bool isLike);
        Task<bool> RemoveLikeAsync(int userId, int entryId);
        Task<int> GetLikeCountForEntryAsync(int entryId, bool isLike);
        Task<int> GetLikeCountForCommentAsync(int commentId, bool isLike);
        Task<Like?> GetUserLikeForEntryAsync(int userId, int entryId);

        Task<int> GetTotalLikesForUserAsync(int userId);
        Task<int> GetTotalDislikesForUserAsync(int userId);

        Task<bool> LikeBlogAsync(int userId, int blogId, bool isLike);
        Task<int> GetLikeCountForBlogAsync(int blogId, bool isLike);
        Task<Like?> GetUserLikeForBlogAsync(int userId, int blogId);

    }
}
