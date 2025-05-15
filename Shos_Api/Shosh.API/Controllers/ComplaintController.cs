using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shosh.API.ViewModel;
using Shosh.Core.Entities;
using Shosh.Service.IService;
using System.Security.Claims;

namespace Shosh.API.Controllers
{
    [Authorize]
    [Route("api/complaints")]
    [ApiController]
    public class ComplaintController : ControllerBase
    {
        private readonly IComplaintService _complaintService;

        public ComplaintController(IComplaintService complaintService)
        {
            _complaintService = complaintService;
        }

        // 📌 **Şikayet Ekleme**
        [HttpPost]
        public async Task<IActionResult> SubmitComplaint(int targetUserId, [FromBody] ComplaintRequestDto request)
        {
            var userId = int.Parse(User.Identity.Name); // Kullanıcı ID’yi JWT’den al
            await _complaintService.AddComplaintAsync(userId, targetUserId, request.Content, request.Content);
            return Ok("Şikayet başarıyla oluşturuldu.");
        }

        // 📌 **Tüm Şikayetleri Listele**
        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet]
        public async Task<IActionResult> GetAllComplaints()
        {
            var complaints = await _complaintService.GetAllComplaintsAsync();
            return Ok(complaints);
        }

        // 📌 **Şikayeti Çöz**
        [Authorize(Roles = "Admin")]
        [HttpPost("{complaintId}/resolve")]
        public async Task<IActionResult> ResolveComplaint(int complaintId)
        {
            var result = await _complaintService.ResolveComplaintAsync(complaintId);
            if (!result) return NotFound("Şikayet bulunamadı.");
            return Ok("Şikayet başarıyla çözüldü.");
        }
    }
}
