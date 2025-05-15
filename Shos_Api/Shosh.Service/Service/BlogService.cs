using Shosh.Core.Entities;
using Shosh.Data.IRepository;
using Shosh.Service.Dto;
using Shosh.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Service.Service
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogService(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<List<Blog>> GetAllBlogsAsync()
        {
            return await _blogRepository.GetAllBlogsAsync();
        }

        public async Task<Blog?> GetBlogByIdAsync(int blogId)
        {
            return await _blogRepository.GetBlogByIdAsync(blogId);
        }

        public async Task<List<Blog>> GetBlogsByUserAsync(int userId)
        {
            return await _blogRepository.GetBlogsByUserAsync(userId);
        }

        public async Task<bool> AddBlogAsync(int userId, string title, string content)
        {
            var blog = new Blog
            {
                UserId = userId,
                Title = title,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };

            await _blogRepository.AddBlogAsync(blog);
            return true;
        }

        public async Task<bool> UpdateBlogAsync(int blogId, int userId, string title, string content)
        {
            var blog = await _blogRepository.GetBlogByIdAsync(blogId);
            if (blog == null || blog.UserId != userId) return false;

            blog.Title = title;
            blog.Content = content;
            await _blogRepository.UpdateBlogAsync(blog);

            return true;
        }

        public async Task<bool> DeleteBlogAsync(int blogId, int userId)
        {
            var blog = await _blogRepository.GetBlogByIdAsync(blogId);
            if (blog == null || blog.UserId != userId) return false;

            await _blogRepository.DeleteBlogAsync(blogId);
            return true;
        }

        public async Task<List<BlogDto>> GetMostLikedBlogsAsync(int userId)
        {
            var blogs = await _blogRepository.GetMostLikedBlogsAsync(userId);
            return blogs.Select(b => new BlogDto
            {
                Id = b.Id,
                Title = b.Title,
                LikeCount = b.Likes.Count(l => l.IsLike),
                DislikeCount = b.Likes.Count(l => !l.IsLike),
                CreatedAt = b.CreatedAt
            }).ToList();
        }

    

    }
}
