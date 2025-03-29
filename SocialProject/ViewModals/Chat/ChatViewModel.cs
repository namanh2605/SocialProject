using SocialProject.Data.Models;

namespace SocialProject.ViewModals.Chat
{
    public class ChatViewModel
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string ReceiverFullName { get; set; }  
        public string ReceiverProfilePictureUrl { get; set; } 
        public List<Message> Messages { get; set; }
        public List<User> Users { get; set; }
    }


}
