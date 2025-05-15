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
    public class UserFollowRepository : IUserFollowRepository
    {
        private readonly ApplicationDbContext _context;

        public UserFollowRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddFollowAsync(UserFollow userFollow)
        {
            await _context.UserFollows.AddAsync(userFollow);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveFollowAsync(int followerId, int followingId)
        {
            var follow = await _context.UserFollows
                .FirstOrDefaultAsync(uf => uf.FollowerId == followerId && uf.FollowingId == followingId);

            if (follow == null)
                return false;

            _context.UserFollows.Remove(follow);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsFollowingAsync(int followerId, int followingId)
        {
            return await _context.UserFollows
                .AnyAsync(uf => uf.FollowerId == followerId && uf.FollowingId == followingId);
        }

        public async Task<List<User>> GetFollowersAsync(int userId)
        {
            return await _context.UserFollows
                .Where(uf => uf.FollowingId == userId)
                .Include(uf => uf.Follower)  // Takip eden kullanıcıyı getir
                .Select(uf => uf.Follower)
                .ToListAsync();
        }

        public async Task<List<User>> GetFollowingAsync(int userId)
        {
            return await _context.UserFollows
                .Where(uf => uf.FollowerId == userId)
                .Include(uf => uf.Following) // Takip edilen kullanıcıyı getir
                .Select(uf => uf.Following)
                .ToListAsync();
        }
    }
}