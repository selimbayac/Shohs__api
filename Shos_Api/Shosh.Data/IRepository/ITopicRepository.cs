using Shosh.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Data.IRepository
{
    public interface ITopicRepository
    {
        Task<List<Topic>> GetAllTopicsAsync(); // 📌 Tüm başlıkları getir
        Task<Topic> GetTopicByIdAsync(int topicId); // 📌 ID’ye göre başlık getir
        Task<bool> TopicExistsAsync(string title); // 📌 Başlık var mı kontrol et
        Task AddTopicAsync(Topic topic); // 📌 Yeni başlık ekle
        Task UpdateTopicAsync(Topic topic); // 📌 Başlığı güncelle
        Task DeleteTopicAsync(Topic topic); // 📌 Başlığı sil
       
    }
}
