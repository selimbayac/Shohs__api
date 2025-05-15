using Shosh.Core.Entities;
using Shosh.Data.IRepository;
using Shosh.Data.Repository;
using Shosh.Service.Dto;
using Shosh.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Service.Service
{
    public class EntryService : IEntryService
    {
        private readonly IEntryRepository _entryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserFollowRepository _userFollowRepository;
        private readonly INotificationService _notificationService;
        private readonly ITopicRepository _topicRepository;
        private readonly ITopicService _topicService;
        private readonly IUserService _userService;
        private readonly ILikeService _likeService;
        public EntryService(IEntryRepository entryRepository, IUserRepository userRepository, IUserFollowRepository userFollowRepository, INotificationService notificationService, ITopicRepository topicRepository, ITopicService topicService, IUserService userService, ILikeService likeService)
        {
            _entryRepository = entryRepository;
            _userRepository = userRepository;
            _userFollowRepository = userFollowRepository;
            _notificationService = notificationService;
            _topicRepository = topicRepository;
            _topicService = topicService;
            _userService = userService;
            _likeService = likeService;
        }

        // 📌 **Tüm Entry'leri Getir**
        public async Task<List<EntryDto>> GetAllEntriesAsync()
        {
            var entries = await _entryRepository.GetAllEntriesAsync();

            return entries.Select(e => new EntryDto
            {
                Id = e.Id,
                Content = e.Content,
                CreatedAt = e.CreatedAt,
                UserId = e.UserId,
                Username = e.User?.Nickname ?? "Bilinmeyen Kullanıcı", // Kullanıcı null ise default değer
                LikeCount = e.Likes.Count(l => l.IsLike),
                DislikeCount = e.Likes.Count(l => !l.IsLike)
            }).ToList();
        }

        // 📌 **Tekil Entry Getir**
        public async Task<Entry?> GetEntryByIdAsync(int entryId)
        {
            return await _entryRepository.GetEntryByIdAsync(entryId);
        }

        // 📌 **Kullanıcıya Ait Entry'leri Getir**
        public async Task<List<EntryDto>> GetEntriesByUserAsync(int userId, int page, int pageSize)
        {
            var entries = await _entryRepository.GetEntriesByUserAsync(userId, page, pageSize);

            return entries.Select(e => new EntryDto
            {
                Id = e.Id,
                Content = e.Content,
                LikeCount = e.Likes.Count(l => l.IsLike),
                DislikeCount = e.Likes.Count(l => !l.IsLike),
                CreatedAt = e.CreatedAt
            }).ToList();
        }


        // 📌 **Yeni Entry Ekle**
        public async Task<bool> AddEntryAsync(int userId, int topicId, string content)
        {
            var entry = new Entry
            {
                UserId = userId,
                TopicId = topicId,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };
            return await _entryRepository.AddEntryAsync(entry);
        }



        // 📌 **Entry Güncelleme (Sadece Sahibi Güncelleyebilir)**
        public async Task<bool> UpdateEntryAsync(int entryId, int userId, string newContent)
        {
            var entry = await _entryRepository.GetEntryByIdAsync(entryId);
            if (entry == null || entry.UserId != userId) return false;

            entry.Content = newContent;
            await _entryRepository.UpdateEntryAsync(entry);
            return true;
        }

        // 📌 **Entry Silme (Sadece Sahibi, Admin veya Moderatör)**  
        public async Task<bool> DeleteEntryAsync(int entryId, int userId, string userRole)
        {
            var entry = await _entryRepository.GetEntryByIdAsync(entryId);
            if (entry == null) return false;

            // Eğer giriş yapan kullanıcı, entry sahibiyse veya admin/moderatörse silebilir
            if (entry.UserId != userId && userRole != "Admin" && userRole != "Moderator")
                return false;

            await _entryRepository.DeleteEntryAsync(entryId);
            return true;
        }
        public async Task<bool> DeleteEntryByAdminAsync(int entryId)
        {
            var entry = await _entryRepository.GetEntryByIdAsync(entryId);
            if (entry == null) return false;

            await _entryRepository.DeleteEntryAsync(entryId);
            return true;
        }
        public async Task<List<EntryDto>> GetMostLikedEntriesAsync(int userId, int page = 1, int pageSize = 5)
        {
            var entries = await _entryRepository.GetEntriesByUserAsync(userId, page, pageSize);

            // En çok beğenilenleri sıralıyoruz
            return entries
                .OrderByDescending(e => e.Likes.Count(l => l.IsLike)) // Beğeniye göre sıralama
                .Select(e => new EntryDto
                {
                    Id = e.Id,
                    Content = e.Content,
                    LikeCount = e.Likes.Count(l => l.IsLike),
                    DislikeCount = e.Likes.Count(l => !l.IsLike),
                    CreatedAt = e.CreatedAt
                })
                .ToList();
        }
        public async Task<List<EntryDto>> GetMostDislikedEntriesAsync(int userId, int page = 1, int pageSize = 5)
        {
            var entries = await _entryRepository.GetEntriesByUserAsync(userId, page, pageSize);

            // En çok dislike alanları sıralıyoruz
            return entries
                .OrderByDescending(e => e.Likes.Count(l => !l.IsLike)) // Dislike'a göre sıralama
                .Select(e => new EntryDto
                {
                    Id = e.Id,
                    Content = e.Content,
                    LikeCount = e.Likes.Count(l => l.IsLike),
                    DislikeCount = e.Likes.Count(l => !l.IsLike),
                    CreatedAt = e.CreatedAt
                })
                .ToList();
        }

        public async Task<List<Entry>> GetEntriesByTopicIdAsync(int topicId)
        {
            return await _entryRepository.GetEntriesByTopicIdAsync(topicId);
        }


        public async Task<List<EntryDto>> GetEntriesByUserIdAsync(int userId, int page, int pageSize)
        {
            // Kullanıcının yazdığı entry'leri alıyoruz ve User ilişkisini de dahil ediyoruz
            var entries = await _entryRepository.GetEntriesByUserIdAsync(userId, page, pageSize);

            // Entry başlıklarını ve beğeni sayısını alıyoruz
            var entryDtos = new List<EntryDto>();

            foreach (var entry in entries)
            {
                // Başlık bilgisini alıyoruz
                var topic = await _topicService.GetTopicByIdAsync(entry.TopicId);

                // Beğeni ve dislike sayısını alıyoruz
                var likeCount = await _likeService.GetLikeCountForEntryAsync(entry.Id, true);  // Beğeni sayısı
                var dislikeCount = await _likeService.GetLikeCountForEntryAsync(entry.Id, false);  // Dislike sayısı

                // Kullanıcı bilgilerini alıyoruz (Nicknames ve beğeni sayısı)
                var username = entry.User?.Nickname ?? "Bilinmeyen Kullanıcı";

                // DTO'yu oluşturuyoruz
                entryDtos.Add(new EntryDto
                {
                    Id = entry.Id,
                    Content = entry.Content,
                    TopicTitle = topic?.Title ?? "Bilinmeyen Başlık",
                    CreatedAt = entry.CreatedAt,
                    Username = username,  // Kullanıcı adı
                    LikeCount = likeCount,   // Beğeni sayısı
                    DislikeCount = dislikeCount   // Dislike sayısı
                });
            }

            return entryDtos;
        }
        public async Task<List<EntryDto>> GetMostLikedEntriesByUserAsync(int userId, int page = 1, int pageSize = 5)
        {
            // Kullanıcıya ait entry'leri alıyoruz
            var entries = await _entryRepository.GetEntriesByUserIdAsync(userId, page, pageSize);

            foreach (var entry in entries)
            {
                // Entry'ye gelen beğenileri alıyoruz
                var likeCount = await _likeService.GetLikeCountForEntryAsync(entry.Id, true); // Beğenileri say
                var dislikeCount = await _likeService.GetLikeCountForEntryAsync(entry.Id, false); // Dislike'ları say

                // Beğeni ve beğenmeme sayısını entry'ye ekliyoruz
                entry.LikeCount = likeCount;
                entry.DislikeCount = dislikeCount;

                // Kullanıcı bilgisi ekliyoruz
                var username = entry.User?.Nickname ?? "Bilinmeyen Kullanıcı";
            }

            // En çok beğenilen entry'leri beğeni sayısına göre sıralıyoruz
            return entries.OrderByDescending(e => e.LikeCount)
                          .Select(e => new EntryDto
                          {
                              Id = e.Id,
                              Content = e.Content,
                              TopicTitle = e.Topic?.Title ?? "Bilinmeyen Başlık",
                              CreatedAt = e.CreatedAt,
                              Username = e.User.UserName,
                              LikeCount = e.LikeCount,
                              DislikeCount = e.DislikeCount
                          }).ToList();
        }

    }
}
