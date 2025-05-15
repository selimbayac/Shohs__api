using Shosh.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Data.IRepository
{
    public interface IBlogRepository
    {
        Task<List<Blog>> GetAllBlogsAsync();  // 📌 Tüm blogları getir
        Task<Blog?> GetBlogByIdAsync(int blogId);  // 📌 Blog ID ile getir
        Task<List<Blog>> GetBlogsByUserAsync(int userId);  // 📌 Kullanıcının bloglarını getir
        Task AddBlogAsync(Blog blog);  // 📌 Yeni blog ekle
        Task UpdateBlogAsync(Blog blog);  // 📌 Blog güncelle
        Task DeleteBlogAsync(int blogId);  // 📌 Blog sil
        Task<List<Blog>> GetMostLikedBlogsAsync(int userId);
    }
}
