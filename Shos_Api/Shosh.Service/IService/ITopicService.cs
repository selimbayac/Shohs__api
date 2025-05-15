using Shosh.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Service.IService
{
    public interface ITopicService
    {
        Task<List<Topic>> GetAllTopicsAsync(); // 📌 Tüm başlıkları getir
        Task<Topic> GetTopicByIdAsync(int topicId); // 📌 ID’ye göre başlık getir
        Task<bool> AddTopicAsync(int userId, string title); // 📌 Başlık oluştur
        Task<bool> UpdateTopicAsync(int topicId, int userId, string newTitle); // 📌 Başlık güncelle
        Task<bool> DeleteTopicAsync(int topicId, string userRole); // 📌 Başlık sil (Admin)
    }
}
