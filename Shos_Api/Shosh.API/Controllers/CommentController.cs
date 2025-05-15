using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shosh.API.ViewModel;
using Shosh.Service.IService;
using System.Security.Claims;

namespace Shosh.API.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // 📌 **Bir Entry'ye Ait Tüm Yorumları Getir**
        [HttpGet("entry/{entryId}")]
        public async Task<IActionResult> GetCommentsByEntryId(int entryId)
        {
            var comments = await _commentService.GetCommentsByEntryIdAsync(entryId);
            return Ok(comments);
        }

        // 📌 **Yeni Yorum Ekle (Sadece Giriş Yapmış Kullanıcılar)**
        [Authorize]
        [HttpPost("entry/{entryId}")]
        public async Task<IActionResult> AddComment(int entryId, [FromBody] CommentRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Content))
                return BadRequest("Yorum içeriği boş olamaz.");

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            bool success = await _commentService.AddCommentAsync(userId, entryId, request.Content);

            if (!success) return BadRequest("Yorum eklenemedi!");
            return Ok(new { Message = "Yorum başarıyla eklendi!" });
        }

        // 📌 **Yorum Silme (Sadece Sahibi veya Admin/Moderatör)**
        [Authorize]
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            string userRole = User.FindFirstValue(ClaimTypes.Role);

            bool success = await _commentService.DeleteCommentAsync(commentId, userId, userRole);
            if (!success) return BadRequest("Bu yorumu silme yetkiniz yok!");

            return Ok(new { Message = "Yorum başarıyla silindi!" });
        }
    }
}
