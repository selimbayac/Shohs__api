using Microsoft.EntityFrameworkCore;
using Shosh.Core.Entities;
using Shosh.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Data.Repository
{
    public class BlogRepository : IBlogRepository
    {
        private readonly ApplicationDbContext _context;

        public BlogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Blog>> GetAllBlogsAsync()
        {
            return await _context.Blogs
                .Include(b => b.User)  // 📌 Blog sahibini getir
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<Blog?> GetBlogByIdAsync(int blogId)
        {
            return await _context.Blogs
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == blogId);
        }

        public async Task<List<Blog>> GetBlogsByUserAsync(int userId)
        {
            return await _context.Blogs
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task AddBlogAsync(Blog blog)
        {
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBlogAsync(Blog blog)
        {
            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBlogAsync(int blogId)
        {
            var blog = await _context.Blogs.FindAsync(blogId);
            if (blog != null)
            {
                _context.Blogs.Remove(blog);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Blog>> GetMostLikedBlogsAsync(int userId)
        {
            return await _context.Blogs
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.Likes.Count(l => l.IsLike))
                .ThenByDescending(b => b.CreatedAt)
                .ToListAsync();
        }
     
    }
}
