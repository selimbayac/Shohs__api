using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shosh.API.ViewModel;
using Shosh.Service.IService;
using Shosh.Service.Service;
using System.Security.Claims;

namespace Shosh.API.Controllers
{
    [Route("api/entries")]
    [ApiController]
    public class EntryController : ControllerBase
    {
        private readonly IEntryService _entryService;
        private readonly IBanService _banService;
        private readonly IUserService _userService;
        private readonly ITopicService _topicService;
        public EntryController(IEntryService entryService, IBanService banService, IUserService userService, ITopicService topicService)
        {
            _entryService = entryService;
            _banService = banService;
            _userService = userService;
            _topicService = topicService;
        }

        // 📌 **Tüm Entry'leri Getir**
        [HttpGet]
        public async Task<IActionResult> GetAllEntries()
        {
            var entries = await _entryService.GetAllEntriesAsync();
            return Ok(entries);
        }

        // 📌 **Kullanıcıya Ait Entry'leri Getir (Düzenleme)**
        // Bu metodun yolu değiştirilmiş olmalı, çakışmayı engellemek için
        [HttpGet("users/{userId}/entries/{page}/{pageSize}")]
        public async Task<IActionResult> GetEntriesByUser([FromRoute] int userId, [FromRoute] int page = 1, [FromRoute] int pageSize = 5)
        {
            var entries = await _entryService.GetEntriesByUserAsync(userId, page, pageSize);
            return Ok(entries);
        }

     

        // 📌 **Tekil Entry Getir**
        [HttpGet("{entryId}")]
        public async Task<IActionResult> GetEntry(int entryId)
        {
            var entry = await _entryService.GetEntryByIdAsync(entryId);
            if (entry == null) return NotFound("Entry bulunamadı.");
            return Ok(entry);
        }

        // 📌 **Entry Ekleme (Sadece Giriş Yapmış Kullanıcı)**
        [Authorize] // API'ye giriş yapmamış kullanıcı erişemez
        [HttpPost]
        public async Task<IActionResult> AddEntry([FromBody] EntryCreateDto request)
        {
            Console.WriteLine($"📢 API'ye Entry Ekleme İsteği -> TopicId: {request.TopicId}, Content: {request.Content}");

            if (string.IsNullOrWhiteSpace(request.Content) || request.TopicId <= 0)
                return BadRequest("Geçersiz TopicId veya içerik boş olamaz.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Kullanıcı doğrulanamadı.");

            bool success = await _entryService.AddEntryAsync(int.Parse(userId), request.TopicId, request.Content);
            return success ? Ok("Entry başarıyla eklendi!") : BadRequest("Entry eklenemedi.");
        }

        // 📌 **Entry Güncelleme (Sadece Entry Sahibi Güncelleyebilir)**
        [Authorize]
        [HttpPut("{entryId}")]
        public async Task<IActionResult> UpdateEntry(int entryId, [FromBody] EntryUpdateDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Content))
                return BadRequest("Yeni içerik boş olamaz.");

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            bool success = await _entryService.UpdateEntryAsync(entryId, userId, request.Content);

            if (!success) return BadRequest("Güncelleme başarısız. Sadece kendi entry'lerinizi güncelleyebilirsiniz.");
            return Ok(new { Message = "Entry başarıyla güncellendi!" });
        }

        // 📌 **Entry Silme (Sadece Entry Sahibi veya Admin/Moderatör)**
        [Authorize]
        [HttpDelete("{entryId}")]
        public async Task<IActionResult> DeleteEntry(int entryId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            string userRole = User.FindFirstValue(ClaimTypes.Role);

            bool success = await _entryService.DeleteEntryAsync(entryId, userId, userRole);
            if (!success) return BadRequest("Entry silinemedi. Yetkiniz yok.");

            return Ok(new { Message = "Entry başarıyla silindi!" });
        }

        // Kullanıcıya ait Entry'leri getir
        [HttpGet("users/{userId}/entries-all")]
        public async Task<IActionResult> GetEntriesByUserIdAsync(int userId, int page = 1, int pageSize = 10)
        {
            try
            {
                // Kullanıcının yazdığı entry'leri ve başlık bilgilerini alıyoruz
                var entries = await _entryService.GetEntriesByUserIdAsync(userId, page, pageSize);

                if (entries == null || !entries.Any())
                {
                    return NotFound("Kullanıcının yazdığı entry'ler bulunamadı.");
                }

                return Ok(entries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        [HttpGet("topics/{topicId}/entries")]
        public async Task<IActionResult> GetEntriesByTopicIdAsync(int topicId)
        {
            var entries = await _entryService.GetEntriesByTopicIdAsync(topicId);

            if (entries == null || !entries.Any())
            {
                return NotFound("Başlık altında entry bulunamadı.");
            }

            return Ok(entries);
        }
        // Kullanıcının entry'lerini ve başlık bilgilerini sayfalı şekilde al
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetEntriesWithTopicsByUserId(int userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            // Kullanıcının entry'lerini ve başlık bilgilerini alıyoruz
            var entries = await _entryService.GetEntriesByUserIdAsync(userId, page, pageSize);

            if (entries == null || !entries.Any())
            {
                return NotFound("Kullanıcının entry'leri bulunamadı.");
            }

            // Başlık bilgileri her entry ile ilişkilendirilecek
            // Örneğin, her entry'nin altında Topic (başlık) bilgisini görmek istiyoruz.

            return Ok(entries); // Entry'ler ve başlık bilgileri ile dönecek
        }
        //en çok beğenilen
        [HttpGet("users/{userId}/most-liked-entries")]
        public async Task<IActionResult> GetMostLikedEntriesByUser(int userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var entries = await _entryService.GetMostLikedEntriesByUserAsync(userId, page, pageSize);
            return Ok(entries);
        }

    }
}
