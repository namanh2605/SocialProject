using Microsoft.EntityFrameworkCore;
using SocialProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialProject.Data.Services
{
    public class AdminService : IAdminService
    {
        private readonly SocialMediaContext _context;
        public AdminService(SocialMediaContext context)
        {
            _context = context;
        }
        public async Task<List<Post>> GetReportedPostsAsync()
        {
            var posts = await _context.Posts
                .Include(n => n.User)
                .Where(n => n.NrOfReports > 0 && !n.IsDeleted)
                .ToListAsync();

            if (posts == null || posts.Count == 0)
            {
                Console.WriteLine("No reported posts found.");
            }

            return posts;
        }

        public async Task ApproveReportAsync(int postId)
        {
            var postDb = await _context.Posts.FirstOrDefaultAsync(n => n.Id == postId);

            if (postDb != null)
            {
                postDb.IsDeleted = true;
                _context.Posts.Update(postDb);
                await _context.SaveChangesAsync();
            }
        }
        public async Task RejectReportAsync(int postId)
        {
            var postDb = await _context.Posts.FirstOrDefaultAsync(n => n.Id == postId);

            if (postDb != null)
            {
                postDb.NrOfReports = 0;
                _context.Posts.Update(postDb);
                await _context.SaveChangesAsync();
            }

            var postReports = await _context.Reports.Where(n => n.PostId == postId).ToListAsync();
            if (postReports.Any())
            {
                _context.Reports.RemoveRange(postReports);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<int> GetTotalPostsAsync()
        {
            return await _context.Posts.CountAsync();
        }


        public async Task<int> GetTotalUsersAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<int> GetTotalLikesAsync()
        {
            return await _context.Likes.CountAsync();
        }

        public async Task<int> GetTotalCommentsAsync()
        {
            return await _context.Comments.CountAsync();
        }

        public async Task<int> GetTotalFriendRequestsAsync()
        {
            return await _context.FriendRequests.CountAsync();
        }

        public async Task<DashboardStatistics> GetDashboardStatisticsAsync()
        {
            return new DashboardStatistics
            {
                TotalPosts = await GetTotalPostsAsync(),
                TotalUsers = await GetTotalUsersAsync(),
                TotalLikes = await GetTotalLikesAsync(),
                TotalComments = await GetTotalCommentsAsync(),
                TotalFriendRequests = await GetTotalFriendRequestsAsync()
            };
        }
    }

    }
