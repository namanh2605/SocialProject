using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialProject.Controllers.Base;
using SocialProject.Data.Models;
using SocialProject.Data.Services;
using SocialProject.ViewModals.Users;

namespace SocialProject.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUsersService _userService;
        private readonly UserManager<User> _userManager;
        public UsersController(IUsersService usersService, UserManager<User> userManager)
        {
            _userService = usersService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var userPosts = await _userService.GetUserPosts(userId);

            var userProfileVM = new GetUserProfileVM()
            {
                User = user,
                Posts = userPosts
            };

            return View(userProfileVM);
        }
    }
}