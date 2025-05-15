using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shosh.API.ViewModel;
using Shosh.Data.IRepository;
using Shosh.Service.IService;
using Shosh.Service.Service;

namespace Shosh.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IComplaintService _complaintService;
        private readonly IBanService _banService;
        private readonly INotificationService _notificationService;
        private readonly IEntryService _entryService;

        public AdminController(IUserService userService, IComplaintService complaintService, IBanService banService, INotificationService notificationService, IEntryService entryService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _complaintService = complaintService ?? throw new ArgumentNullException(nameof(complaintService));
            _banService = banService;
            _notificationService = notificationService;
            _entryService = entryService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("ban-user/{userId}")]
        public async Task<IActionResult> BanUser(int userId, [FromBody] BanUserDto request)
        {
            var result = await _banService.BanUserAsync(userId, request.Reason, request.Days);
            if (!result) return NotFound("Kullanıcı bulunamadı.");

            // 📌 Banlanan kullanıcıya bildirim gönderelim
            string durationMessage = request.Days.HasValue ? $"{request.Days} gün" : "Süresiz";
            await _notificationService.SendNotificationAsync(userId, $"Admin tarafından banlandınız. Sebep: {request.Reason}. Süre: {durationMessage}");

            return Ok($"Kullanıcı {durationMessage} boyunca banlandı. Sebep: {request.Reason}");
        }



        [HttpGet("complaints")]
        public async Task<IActionResult> GetAllComplaints()
        {
            var complaints = await _complaintService.GetAllComplaintsAsync();
            return Ok(complaints);
        } 

        [Authorize(Roles = "Admin")]
        [HttpPost("unban-user/{userId}")]
        public async Task<IActionResult> UnbanUser(int userId)
        {
            var result = await _userService.UnbanUserAsync(userId);
            if (!result) return NotFound("Banlı kullanıcı bulunamadı.");

            return Ok(new { Message = $"Kullanıcının banı başarıyla kaldırıldı!" });
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet("get-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-entry/{entryId}")]
        public async Task<IActionResult> DeleteEntry(int entryId)
        {
            bool result = await _entryService.DeleteEntryByAdminAsync(entryId);
            if (!result) return NotFound("Entry bulunamadı veya silinemedi.");

            return Ok("Entry başarıyla silindi.");
        }

        [HttpPost("resolve-complaint/{complaintId}")]
        public async Task<IActionResult> ResolveComplaint(int complaintId)
        {
            var result = await _complaintService.ResolveComplaintAsync(complaintId);
            if (!result) return NotFound("Şikayet bulunamadı veya entry/yorum/blog zaten silinmiş.");

            return Ok("Şikayet edilen içerik başarıyla kaldırıldı.");
        }
    }
}
