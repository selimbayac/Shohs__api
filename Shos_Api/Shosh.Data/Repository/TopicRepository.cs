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
    public class TopicRepository : ITopicRepository
    {
        private readonly ApplicationDbContext _context;

        public TopicRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Topic>> GetAllTopicsAsync()
        {
            return await _context.Topics
        .Include(t => t.Entries)  // 📌 Başlığın entry'lerini getiriyoruz
        .ThenInclude(e => e.User)  // 📌 Entryleri yazan kullanıcıları da ekleyelim
        .ToListAsync();
        }

        //public async Task<Topic?> GetTopicByIdAsync(int topicId)
        //{
        //    return await _context.Topics
        //.Include(t => t.Entries)  // 📌 Başlık ile beraber Entry'leri getir
        //.ThenInclude(e => e.User)  // 📌 Entry'leri yazan kullanıcı bilgilerini de getir
        //.FirstOrDefaultAsync(t => t.Id == topicId);
        //}

        public async Task<bool> TopicExistsAsync(string title)
        {
            return await _context.Topics.AnyAsync(t => t.Title.ToLower() == title.ToLower());
        }

        public async Task AddTopicAsync(Topic topic)
        {
            await _context.Topics.AddAsync(topic);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTopicAsync(Topic topic)
        {
            _context.Topics.Update(topic);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTopicAsync(Topic topic)
        {
            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();
        }
        // Başlık ID'sine göre başlık bilgisi almak için
        public async Task<Topic> GetTopicByIdAsync(int topicId)
        {
            return await _context.Topics
                                 .FirstOrDefaultAsync(t => t.Id == topicId);
        }
    }
}
