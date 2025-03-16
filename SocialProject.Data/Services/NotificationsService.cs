using SocialProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialProject.Data.Services
{
    public class NotificationsService : INotificationsService
    {
        private readonly SocialMediaContext _context;
        public NotificationsService(SocialMediaContext context)
        {
            _context = context;
        }

        public async Task AddNewNotificationAsync(int userId, string message, string notificationType)
        {
            var newNotification = new Notification()
            {
                UserId = userId,
                Message = message,
                Type = notificationType,
                IsRead = false,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };

            await _context.Notifications.AddAsync(newNotification);
            await _context.SaveChangesAsync();
        }
    }
}