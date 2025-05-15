using Shosh.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Data.IRepository
{
    public interface IEntryRepository
    {
        Task<Entry?> GetEntryByIdAsync(int entryId);
        Task<List<Entry>> GetAllEntriesAsync();
        Task<List<Entry>> GetEntriesByUserAsync(int userId, int page, int pageSize);
        Task<bool> AddEntryAsync(Entry entry);
        Task UpdateEntryAsync(Entry entry);
        Task DeleteEntryAsync(int entryId);
        Task<List<Entry>> GetMostLikedEntriesAsync(int userId);
        Task<List<Entry>> GetEntriesByTopicIdAsync(int topicId);
        Task<List<Entry>> GetEntriesByUserIdAsync(int userId, int page = 1, int pageSize = 10);
        Task<Topic> GetTopicByIdAsync(int topicId);
    }
}
