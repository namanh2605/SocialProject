using Microsoft.EntityFrameworkCore;
using SocialProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialProject.Data.Services
{
    public class UsersService : IUsersService
    {
        private readonly SocialMediaContext _appDbContext;
        public UsersService(SocialMediaContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<User> GetUser(int loggedInUserId)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(n => n.Id == loggedInUserId) ?? new User();
        }
        public async Task UpdateUserProfilePicture(int loggedInUserId, string profilePictureUrl)
        {
            var userDb = await _appDbContext.Users.FirstOrDefaultAsync(n => n.Id == loggedInUserId);

            if (userDb != null)
            {
                userDb.ProfilePictureUrl = profilePictureUrl;
                _appDbContext.Users.Update(userDb);
                await _appDbContext.SaveChangesAsync();
            }
        }
        public async Task<List<Post>> GetUserPosts(int userId)
        {
            var allPosts = await _appDbContext.Posts
                .Where(n => n.UserId == userId && n.Reports.Count < 5 && !n.IsDeleted)
                .Include(n => n.User)
                .Include(n => n.Likes)
                .Include(n => n.Favorites)
                .Include(n => n.Comments).ThenInclude(n => n.User)
                .Include(n => n.Reports)
                .OrderByDescending(n => n.DateCreated)
                .ToListAsync();

            return allPosts;
        }
        public async Task<List<User>> SearchUsersAsync(string query, int userId)
        {
            return await _appDbContext.Users
                .Where(u => u.FullName.Contains(query) || u.Email.Contains(query))
                .ToListAsync();
        }
    }
}