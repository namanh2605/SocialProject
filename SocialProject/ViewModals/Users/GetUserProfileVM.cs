using SocialProject.Data.Models;

namespace SocialProject.ViewModals.Users
{
    public class GetUserProfileVM
    {
        public User User { get; set; }
        public List<Post> Posts { get; set; }
    }
}
