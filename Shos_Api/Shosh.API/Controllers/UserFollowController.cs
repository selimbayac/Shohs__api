using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shosh.Service.IService;
using System.Security.Claims;

namespace Shosh.API.Controllers
{
    [Route("api/follow")]
    [ApiController]
    public class UserFollowController : ControllerBase
    {
        private readonly IUserFollowService _userFollowService;

        public UserFollowController(IUserFollowService userFollowService)
        {
            _userFollowService = userFollowService;
        }

        // 📌 **Bir kullanıcıyı takip et**
        [Authorize]
        [HttpPost("{followingId}")]
        public async Task<IActionResult> FollowUser(int followingId)
        {
            var followerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            bool success = await _userFollowService.FollowUserAsync(followerId, followingId);
            if (!success) return BadRequest("Zaten takip ediliyor!");

            return Ok("Kullanıcı başarıyla takip edildi!");
        }

        // 📌 **Bir kullanıcıyı takipten çıkar**
        [Authorize]
        [HttpDelete("{followingId}")]
        public async Task<IActionResult> UnfollowUser(int followingId)
        {
            var followerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            bool success = await _userFollowService.UnfollowUserAsync(followerId, followingId);
            if (!success) return BadRequest("Zaten takip edilmiyor!");

            return Ok("Takipten çıkarıldı!");
        }

        // 📌 **Takip kontrolü**
        [Authorize]
        [HttpGet("is-following/{followingId}")]
        public async Task<IActionResult> IsFollowing(int followingId)
        {
            var followerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            bool isFollowing = await _userFollowService.IsFollowingAsync(followerId, followingId);
            return Ok(new { isFollowing });
        }

        // 📌 **Takipçileri getir**
        [HttpGet("{userId}/followers")]
        public async Task<IActionResult> GetFollowers(int userId)
        {
            var followers = await _userFollowService.GetFollowersAsync(userId);
            return Ok(followers);
        }

        // 📌 **Takip edilenleri getir**
        [HttpGet("{userId}/following")]
        public async Task<IActionResult> GetFollowing(int userId)
        {
            var following = await _userFollowService.GetFollowingAsync(userId);
            return Ok(following);
        }
    }
}
