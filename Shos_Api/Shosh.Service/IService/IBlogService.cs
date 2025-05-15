using Shosh.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Service.IService
{
    public interface IBlogService
    {
        Task<List<Blog>> GetAllBlogsAsync();
        Task<Blog?> GetBlogByIdAsync(int blogId);
        Task<List<Blog>> GetBlogsByUserAsync(int userId);
        Task<bool> AddBlogAsync(int userId, string title, string content);
        Task<bool> UpdateBlogAsync(int blogId, int userId, string title, string content);
        Task<bool> DeleteBlogAsync(int blogId, int userId);

 
    }
}
