using Shosh.Core.Entities;
using Shosh.Data.IRepository;
using Shosh.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Service.Service
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;

        public TopicService(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }
        public async Task<List<Topic>> GetAllTopicsAsync()
        {
            return await _topicRepository.GetAllTopicsAsync();
        }

        public async Task<Topic> GetTopicByIdAsync(int topicId)
        {
            return await _topicRepository.GetTopicByIdAsync(topicId);
        }

        public async Task<bool> AddTopicAsync(int userId, string title)
        {
            // Başlık var mı kontrol et
            bool exists = await _topicRepository.TopicExistsAsync(title);
            if (exists) return false;

            var newTopic = new Topic
            {
                Title = title,
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = userId
            };

            await _topicRepository.AddTopicAsync(newTopic);
            return true;
        }

        public async Task<bool> UpdateTopicAsync(int topicId, int userId, string newTitle)
        {
            var topic = await _topicRepository.GetTopicByIdAsync(topicId);
            if (topic == null || topic.CreatedByUserId != userId) return false;

            topic.Title = newTitle;
            await _topicRepository.UpdateTopicAsync(topic);
            return true;
        }

        public async Task<bool> DeleteTopicAsync(int topicId, string userRole)
        {
            var topic = await _topicRepository.GetTopicByIdAsync(topicId);
            if (topic == null || userRole != "Admin") return false;

            await _topicRepository.DeleteTopicAsync(topic);
            return true;
        }

      
    }
}