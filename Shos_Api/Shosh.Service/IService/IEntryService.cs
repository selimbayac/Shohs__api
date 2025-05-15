using Shosh.Core.Entities;
using Shosh.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Service.IService
{
    public interface IEntryService
    {
        Task<List<EntryDto>> GetAllEntriesAsync();
        Task<Entry?> GetEntryByIdAsync(int entryId);
        Task<List<EntryDto>> GetEntriesByUserAsync(int userId, int page, int pageSize);
        Task<bool> AddEntryAsync(int userId, int topicId, string content);
        Task<bool> UpdateEntryAsync(int entryId, int userId, string newContent);
        Task<bool> DeleteEntryAsync(int entryId, int userId, string userRole);
        Task<bool> DeleteEntryByAdminAsync(int entryId);
        Task<List<EntryDto>> GetMostLikedEntriesAsync(int userId, int page, int pageSize);
        Task<List<Entry>> GetEntriesByTopicIdAsync(int topicId);
        Task<List<EntryDto>> GetEntriesByUserIdAsync(int userId, int page , int pageSize);
        Task<List<EntryDto>> GetMostLikedEntriesByUserAsync(int userId, int page = 1, int pageSize = 5);
    }
}
