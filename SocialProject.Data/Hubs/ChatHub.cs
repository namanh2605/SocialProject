using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SocialProject.Data;
using SocialProject.Data.Models;

public class ChatHub : Hub
{
    private readonly SocialMediaContext _context;

    public ChatHub(SocialMediaContext context)
    {
        _context = context;
    }

    public async Task SendMessage(int senderId, int receiverId, string message)
    {
        var msg = new Message
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Content = message,
            SentAt = DateTime.UtcNow
        };

        _context.Messages.Add(msg);
        await _context.SaveChangesAsync();

        await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", senderId, message);
        await Clients.User(senderId.ToString()).SendAsync("ReceiveMessage", senderId, message); 
    }


    public async Task SendPrivateMessage(int receiverId, string message)
    {
        if (int.TryParse(Context.UserIdentifier, out int senderId))
        {
            await SendMessage(senderId, receiverId, message);
        }
        else
        {
            Console.WriteLine("Failed to convert senderId to integer.");
        }
    }
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            await Clients.All.SendAsync("UpdateUserStatus", userId, true);
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            await Clients.All.SendAsync("UpdateUserStatus", userId, false);
        }
        await base.OnDisconnectedAsync(exception);
    }

}
