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
    public class EntryRepository : IEntryRepository
    {
        private readonly ApplicationDbContext _context;

        public EntryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // 📌 **Entry ID'ye Göre Getir**
        public async Task<Entry?> GetEntryByIdAsync(int entryId)
        {
            return await _context.Entries
                .Include(e => e.User) // Kullanıcı bilgisiyle getir
                .Include(e => e.Likes) // Beğenileri getir
                .Include(e => e.Comments) // Yorumları getir
                .ThenInclude(c => c.User) // Yorumu yazan kullanıcıyı getir
                .FirstOrDefaultAsync(e => e.Id == entryId);
        }

        // 📌 **Tüm Entry'leri Getir**
        public async Task<List<Entry>> GetAllEntriesAsync()
        {
            return await _context.Entries
                .Include(e => e.User)
                .Include(e => e.Likes)
                .Include(e => e.Comments)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        // 📌 **Kullanıcıya Ait Entry'leri Getir**
        public async Task<List<Entry>> GetEntriesByUserAsync(int userId, int page, int pageSize)
        {
            return await _context.Entries
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.CreatedAt) // Sayfalama sırasında entry'ler tarihe göre sıralanır
                .Skip((page - 1) * pageSize) // Sayfalama: kaç sayfa atlanacak?
                .Take(pageSize) // Sayfa başına kaç entry dönecek?
                .Include(e => e.User)
                .Include(e => e.Likes)
                .Include(e => e.Topic)
                .ToListAsync();
        }



        // 📌 **Yeni Entry Ekle**
        public async Task<bool> AddEntryAsync(Entry entry)
        {
            await _context.Entries.AddAsync(entry);
            await _context.SaveChangesAsync();
            return true;
        }
        // 📌 **Entry Güncelle**
        public async Task UpdateEntryAsync(Entry entry)
        {
            _context.Entries.Update(entry);
            await _context.SaveChangesAsync();
        }

        // 📌 **Entry Sil**
        public async Task DeleteEntryAsync(int entryId)
        {
            var entry = await _context.Entries.FindAsync(entryId);
            if (entry != null)
            {
                _context.Entries.Remove(entry);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Entry>> GetMostLikedEntriesAsync(int userId)
        {
            return await _context.Entries
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.Likes.Count(l => l.IsLike))
                .ThenByDescending(e => e.CreatedAt)
                .ToListAsync();
        }
        public async Task<List<Entry>> GetEntriesByTopicIdAsync(int topicId)
        {
            return await _context.Entries
                .Where(e => e.TopicId == topicId)
                .Include(e => e.User) // Entry'yi yazan kullanıcıyı da dahil et
                .ToListAsync();
        }
        public async Task<Topic> GetTopicByIdAsync(int topicId)
        {
            return await _context.Topics
                .FirstOrDefaultAsync(t => t.Id == topicId);
        }

        public async Task<List<Entry>> GetEntriesByUserIdAsync(int userId, int page, int pageSize)
        {
            return await _context.Entries
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.CreatedAt) // Sayfalama sırasında entry'ler tarihe göre sıralanır
                .Skip((page - 1) * pageSize) // Sayfalama: kaç sayfa atlanacak?
                .Take(pageSize) // Sayfa başına kaç entry dönecek?
                .Include(e => e.User) // Kullanıcı bilgisi ile birlikte alıyoruz
                .Include(e => e.Likes) // Beğenileri de alıyoruz
                .Include(e => e.Topic) // Başlık bilgilerini de alıyoruz
                .ToListAsync();
        }


    }
}
