using SocialProject.Data.Models;

namespace SocialProject.ViewModals.Friends
{
    public class FriendshipVM
    {
        public List<Friendship> Friends = new List<Friendship>();
        public List<FriendRequest> FriendRequestsSent = new List<FriendRequest>();
        public List<FriendRequest> FriendRequestsReceived = new List<FriendRequest>();
    }
}