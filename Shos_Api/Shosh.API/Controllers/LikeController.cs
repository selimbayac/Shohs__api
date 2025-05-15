using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shosh.API.ViewModel;
using Shosh.Core.Entities;
using Shosh.Service.IService;
using Shosh.Service.Service;
using System.Security.Claims;

namespace Shosh.API.Controllers
{
    [Route("api/likes")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;
        }


        /// <summary>
        /// Entry'ye beğeni veya dislike ekler veya kaldırır.
        /// </summary>
        [Authorize]
        [HttpPost("entry/{entryId}")]
        public async Task<IActionResult> LikeEntry(int entryId, [FromBody] LikeRequestModel model)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var existingLike = await _likeService.GetUserLikeForEntryAsync(userId, entryId);

            if (existingLike != null)
            {
                if (existingLike.IsLike == model.IsLike)
                {
                    bool removed = await _likeService.RemoveLikeAsync(userId, entryId);
                    return removed ? Ok("Beğeni kaldırıldı!") : BadRequest("Beğeni kaldırılamadı.");
                }

                bool updated = await _likeService.LikeEntryAsync(userId, entryId, model.IsLike);
                return updated ? Ok("Beğeni güncellendi!") : BadRequest("Beğeni güncellenemedi.");
            }

            bool added = await _likeService.LikeEntryAsync(userId, entryId, model.IsLike);
            return added ? Ok("Beğeni başarıyla eklendi!") : BadRequest("Beğeni eklenemedi.");
        }

        [Authorize]
        [HttpPost("comment/{commentId}")]
        public async Task<IActionResult> LikeComment(int commentId, [FromBody] LikeRequestModel model)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            bool success = await _likeService.LikeCommentAsync(userId, commentId, model.IsLike);
            return success ? Ok("İşlem başarılı!") : BadRequest("Beğeni eklenemedi.");
        }

        [HttpGet("entry/{entryId}/count")]
        public async Task<IActionResult> GetLikeCountForEntry(int entryId, [FromQuery] bool isLike)
        {
            int count = await _likeService.GetLikeCountForEntryAsync(entryId, isLike);
            return Ok(count);
        }

        [Authorize]
        [HttpGet("entry/{entryId}/user")]
        public async Task<IActionResult> GetUserLikeForEntry(int entryId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userLike = await _likeService.GetUserLikeForEntryAsync(userId, entryId);
            return Ok(userLike?.IsLike);
        }

        /// 📌 **Blog Like / Dislike İşlemi**
        [Authorize]
        [HttpPost("blog/{blogId}")]
        public async Task<IActionResult> LikeBlog(int blogId, [FromBody] LikeRequestModel model)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var existingLike = await _likeService.GetUserLikeForBlogAsync(userId, blogId);

            if (existingLike != null)
            {
                if (existingLike.IsLike == model.IsLike)
                {
                    bool removed = await _likeService.RemoveLikeAsync(userId, blogId);
                    return removed ? Ok("Beğeni kaldırıldı!") : BadRequest("Beğeni kaldırılamadı.");
                }

                bool updated = await _likeService.LikeBlogAsync(userId, blogId, model.IsLike);
                return updated ? Ok("Beğeni güncellendi!") : BadRequest("Beğeni güncellenemedi.");
            }

            bool added = await _likeService.LikeBlogAsync(userId, blogId, model.IsLike);
            return added ? Ok("Beğeni başarıyla eklendi!") : BadRequest("Beğeni eklenemedi.");
        }

        /// 📌 **Blog Beğeni Sayısını Getir**
        [HttpGet("blog/{blogId}/count")]
        public async Task<IActionResult> GetBlogLikeCounts(int blogId)
        {
            int likeCount = await _likeService.GetLikeCountForBlogAsync(blogId, true);
            int dislikeCount = await _likeService.GetLikeCountForBlogAsync(blogId, false);

            return Ok(new { LikeCount = likeCount, DislikeCount = dislikeCount });
        }

        /// 📌 **Kullanıcının Bloga Beğeni Verip Vermediğini Getir**
        [Authorize]
        [HttpGet("blog/{blogId}/user")]
        public async Task<IActionResult> GetUserLikeForBlog(int blogId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userLike = await _likeService.GetUserLikeForBlogAsync(userId, blogId);
            return Ok(userLike?.IsLike);
        }
        [HttpGet("user/{userId}/total-likes")]
        public async Task<IActionResult> GetTotalLikesForUser(int userId)
        {
            int totalLikes = await _likeService.GetTotalLikesForUserAsync(userId);
            return Ok(totalLikes);
        }

        [HttpGet("user/{userId}/total-dislikes")]
        public async Task<IActionResult> GetTotalDislikesForUser(int userId)
        {
            int totalDislikes = await _likeService.GetTotalDislikesForUserAsync(userId);
            return Ok(totalDislikes);
        }
    }
}
