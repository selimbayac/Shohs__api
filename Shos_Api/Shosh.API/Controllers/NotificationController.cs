using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shosh.Service.IService;
using System.Security.Claims;

namespace Shosh.API.Controllers
{
    [Authorize]
    [Route("api/notifications")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // 📌 Kullanıcının Bildirimlerini Getir
        [HttpGet]
        public async Task<IActionResult> GetUserNotifications()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

            if (!int.TryParse(userIdClaim, out int userId)) return BadRequest("Geçersiz kullanıcı ID.");

            var notifications = await _notificationService.GetUserNotificationsAsync(userId);
            return Ok(notifications);
        }

        // 📌 Bildirimi Okundu Olarak İşaretle
        [HttpPost("{notificationId}/read")]
        public async Task<IActionResult> MarkNotificationAsRead(int notificationId)
        {
            await _notificationService.MarkAsReadAsync(notificationId);
            return Ok("Bildirim okundu olarak işaretlendi.");
        }
    }
}
