using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shosh.API.ViewModel;
using Shosh.Service.IService;
using Shosh.Service.Service;
using System.Security.Claims;

namespace Shosh.API.Controllers
{
    [Route("api/topics")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly ITopicService _topicService;
        private readonly IEntryService _entryService;

        public TopicController(ITopicService topicService, IEntryService entryService)
        {
            _topicService = topicService;
            _entryService = entryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTopics()
        {
            var topics = await _topicService.GetAllTopicsAsync();
            return Ok(topics);
        }

        [HttpGet("{topicId}")]
        public async Task<IActionResult> GetTopicById(int topicId)
        {
            var topic = await _topicService.GetTopicByIdAsync(topicId);
            if (topic == null) return NotFound("Başlık bulunamadı.");
            return Ok(topic);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddTopic([FromBody] string title)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            bool success = await _topicService.AddTopicAsync(userId, title);

            if (!success) return BadRequest("Bu başlık zaten mevcut.");
            return Ok("Başlık başarıyla oluşturuldu.");
        }

        [Authorize]
        [HttpPut("{topicId}")]
        public async Task<IActionResult> UpdateTopic(int topicId, [FromBody] string newTitle)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            bool success = await _topicService.UpdateTopicAsync(topicId, userId, newTitle);

            if (!success) return BadRequest("Güncelleme başarısız.");
            return Ok("Başlık başarıyla güncellendi.");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{topicId}")]
        public async Task<IActionResult> DeleteTopic(int topicId)
        {
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            bool success = await _topicService.DeleteTopicAsync(topicId, userRole);

            if (!success) return BadRequest("Silme işlemi başarısız.");
            return Ok("Başlık başarıyla silindi.");
        }

        [HttpGet("with-entries/{topicId}")]
        public async Task<IActionResult> GetTopicWithEntriesAndComments(int topicId)
        {
            var topic = await _topicService.GetTopicByIdAsync(topicId);
            if (topic == null) return NotFound("Başlık bulunamadı.");

            var entries = await _entryService.GetEntriesByTopicIdAsync(topicId);

            foreach (var entry in entries)
            {
                entry.Comments = await _entryService.GetCommentsByEntryIdAsync(entry.Id); // veya _commentService
            }

            topic.Entries = entries;
            return Ok(topic);
        }


    }
}
