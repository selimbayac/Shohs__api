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
    public class UserFollowService : IUserFollowService
    {
        private readonly IUserFollowRepository _userFollowRepository;
        private readonly INotificationService _notificationService;
        public UserFollowService(IUserFollowRepository userFollowRepository, INotificationService notificationService)
        {
            _userFollowRepository = userFollowRepository;
            _notificationService = notificationService;
        }
        public async Task<bool> FollowUserAsync(int followerId, int followingId)
        {
            if (followerId == followingId)
                return false; // Kendi kendini takip edemez

            var isFollowing = await _userFollowRepository.IsFollowingAsync(followerId, followingId);
            if (isFollowing)
                return false; // Zaten takip ediyorsa işlem yapma

            var follow = new UserFollow
            {
                FollowerId = followerId,
                FollowingId = followingId
            };

            await _userFollowRepository.AddFollowAsync(follow);

            // 📌 Bildirim gönder
            await _notificationService.SendNotificationAsync(followingId, "Bir kullanıcı sizi takip etti!");

            return true;
        }

        public async Task<bool> UnfollowUserAsync(int followerId, int followingId)
        {
            return await _userFollowRepository.RemoveFollowAsync(followerId, followingId);
        }

        public async Task<bool> IsFollowingAsync(int followerId, int followingId)
        {
            return await _userFollowRepository.IsFollowingAsync(followerId, followingId);
        }

        public async Task<List<User>> GetFollowersAsync(int userId)
        {
            return await _userFollowRepository.GetFollowersAsync(userId);
        }

        public async Task<List<User>> GetFollowingAsync(int userId)
        {
            return await _userFollowRepository.GetFollowingAsync(userId);
        }
    }
}
