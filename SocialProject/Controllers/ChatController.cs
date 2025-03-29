using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SocialProject.Controllers.Base;
using SocialProject.Data;
using SocialProject.Data.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SocialProject.Data.Constants;
using SocialProject.ViewModals.Chat;
using Microsoft.AspNetCore.Identity;
using SocialProject.Data.Services;

namespace SocialProject.Controllers
{
    [Authorize(Roles = AppRoles.User)]

    public class ChatController : BaseController
    {
        private readonly SocialMediaContext _context;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly UserManager<User> _userManager;


        public ChatController(SocialMediaContext context, IHubContext<ChatHub> hubContext, UserManager<User> userManager)
        {
            _context = context;
            _hubContext = hubContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Chat(int receiverId)
        {
            var senderId = GetUserId();

            if (senderId == null)
            {
                return RedirectToLogin();
            }

            var receiver = await _userManager.FindByIdAsync(receiverId.ToString());

            var model = new ChatViewModel
            {
                SenderId = senderId.Value,
                ReceiverId = receiverId,
                ReceiverFullName = receiver.FullName,
                ReceiverProfilePictureUrl = receiver.ProfilePictureUrl,
                Messages = await _context.Messages
                    .Where(m => (m.SenderId == senderId.Value && m.ReceiverId == receiverId) ||
                                (m.SenderId == receiverId && m.ReceiverId == senderId.Value))
                    .OrderBy(m => m.SentAt)
                    .ToListAsync(),
                Users = await _userManager.Users.ToListAsync() // Lấy danh sách người dùng online
            };

            return View(model);
        }







        [HttpPost]
        public async Task<IActionResult> SendMessage(int receiverId, string message)
        {
            var senderId = GetUserId();
            if (senderId == null)
            {
                return RedirectToLogin();
            }

            var newMessage = new Message
            {
                SenderId = senderId.Value,
                ReceiverId = receiverId,
                Content = message,
                SentAt = DateTime.UtcNow
            };

            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", senderId, message);

            return Ok();
        }
    }
}