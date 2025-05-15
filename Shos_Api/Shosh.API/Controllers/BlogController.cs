using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shosh.API.ViewModel;
using Shosh.Service.IService;
using System.Security.Claims;

namespace Shosh.API.Controllers
{
    [Route("api/blogs")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        // 📌 **Tüm Blogları Getir**
        [HttpGet]
        public async Task<IActionResult> GetAllBlogs()
        {
            var blogs = await _blogService.GetAllBlogsAsync();
            return Ok(blogs);
        }

        // 📌 **Belirli Bir Blogu Getir**
        [HttpGet("{blogId}")]
        public async Task<IActionResult> GetBlogById(int blogId)
        {
            var blog = await _blogService.GetBlogByIdAsync(blogId);
            if (blog == null) return NotFound("Blog bulunamadı.");
            return Ok(blog);
        }

        // 📌 **Belirli Kullanıcının Bloglarını Getir**
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBlogsByUser(int userId)
        {
            var blogs = await _blogService.GetBlogsByUserAsync(userId);
            return Ok(blogs);
        }

        // 📌 **Yeni Blog Ekle**
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddBlog([FromBody] BlogRequestDto request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            bool success = await _blogService.AddBlogAsync(userId, request.Title, request.Content);

            if (!success) return BadRequest("Blog eklenemedi!");
            return Ok("Blog başarıyla eklendi!");
        }

        // 📌 **Blog Güncelle**
        [Authorize]
        [HttpPut("{blogId}")]
        public async Task<IActionResult> UpdateBlog(int blogId, [FromBody] BlogRequestDto request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            bool success = await _blogService.UpdateBlogAsync(blogId, userId, request.Title, request.Content);

            if (!success) return BadRequest("Blog güncellenemedi veya yetkiniz yok!");
            return Ok("Blog başarıyla güncellendi!");
        }

        // 📌 **Blog Sil**
        [Authorize]
        [HttpDelete("{blogId}")]
        public async Task<IActionResult> DeleteBlog(int blogId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            bool success = await _blogService.DeleteBlogAsync(blogId, userId);

            if (!success) return BadRequest("Blog silinemedi veya yetkiniz yok!");
            return Ok("Blog başarıyla silindi!");

        }
    }
}