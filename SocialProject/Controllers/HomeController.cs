using SocialProject.Data;
using SocialProject.Data.Models;
using SocialProject.ViewModals.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialProject.Data;
using SocialProject.Data.Models;
using SocialProject.ViewModals.Home;
using System;
using System.Diagnostics;
using SocialProject.Data.Helpers;
using SocialProject.Data.Services;
using SocialProject.Data.Helpers.Enums;
using Microsoft.AspNetCore.Authorization;
using SocialProject.Controllers.Base;
using Microsoft.AspNetCore.SignalR;
using SocialProject.Data.Hubs;
using SocialProject.Data.Constants;

namespace SocialProject.Controllers
{
    [Authorize(Roles = AppRoles.User)]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostsService _postsService;
        private readonly IHashtagsService _hashtagsService;
        private readonly IFilesService _filesService;
        private readonly INotificationsService _notificationService;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IFriendsService _friendsService;
        private readonly IUsersService _usersService;
        private readonly SocialMediaContext _context;

        public HomeController(ILogger<HomeController> logger,
            IPostsService postsService,
            IHashtagsService hashtagsService,
            IFilesService filesService,
            IHubContext<NotificationHub> hubContext,
            INotificationsService notificationService,
            IFriendsService friendsService,
            IUsersService usersService,
            SocialMediaContext context)
        {
            _logger = logger;
            _postsService = postsService;
            _hashtagsService = hashtagsService;
            _filesService = filesService;
            _hubContext = hubContext;
            _notificationService = notificationService;
            _friendsService = friendsService;
            _usersService = usersService;
            _context = context;

        }


        public async Task<IActionResult> Index()
        {
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null) return RedirectToLogin();

            var allPosts = await _postsService.GetAllPostsAsync(loggedInUserId.Value);

            return View(allPosts);
        }

        public async Task<IActionResult> Details(int postId)
        {
            var post = await _postsService.GetPostByIdAsync(postId);

            if (post == null)
            {
                return View();  // Handle if the post is not found
            }

            return View(post);  // Pass the valid model to the view
        }


        [HttpPost]
        public async Task<IActionResult> CreatePost(PostVM post)
        {
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null) return RedirectToLogin();

            var imageUploadPath = await _filesService.UploadImageAsync(post.Image, ImageFileType.PostImage);

            //Create a new post
            var newPost = new Post
            {
                Content = post.Content,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                ImageUrl = imageUploadPath,
                NrOfReports = 0,
                UserId = loggedInUserId.Value
            };

            await _postsService.CreatePostAsync(newPost);
            await _hashtagsService.ProcessHashtagsForNewPostAsync(post.Content);

            //Redirect to the index page
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TogglePostLike(PostLikeVM postLikeVM)
        {
            var userId = GetUserId();
            var userName = GetUserFullName();
            if (userId == null) return RedirectToLogin();

            var result = await _postsService.TogglePostLikeAsync(postLikeVM.PostId, userId.Value);
            var post = await _postsService.GetPostByIdAsync(postLikeVM.PostId);

            if (result.SendNotification && userId != post.UserId)
                await _notificationService.AddNewNotificationAsync(post.UserId, NotificationType.Like, userName, postLikeVM.PostId);

            return PartialView("Home/_Post", post);
        }

        [HttpPost]
        public async Task<IActionResult> TogglePostFavorite(PostFavoriteVM postFavoriteVM)
        {
            var userId = GetUserId();
            var userName = GetUserFullName();
            if (userId == null) return RedirectToLogin();
            var result = await _postsService.TogglePostFavoriteAsync(postFavoriteVM.PostId, userId.Value);

            var post = await _postsService.GetPostByIdAsync(postFavoriteVM.PostId);

            if (result.SendNotification && userId != post.UserId)
                await _notificationService.AddNewNotificationAsync(post.UserId, NotificationType.Favorite, userName, postFavoriteVM.PostId);


            return PartialView("Home/_Post", post);
        }



        [HttpPost]
        public async Task<IActionResult> TogglePostVisibility(PostVisibilityVM postVisibilityVM)
        {
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null) return RedirectToLogin();
            await _postsService.TogglePostVisibilityAsync(postVisibilityVM.PostId, loggedInUserId.Value);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPostComment(PostCommentVM postCommentVM)
        {
            var userId = GetUserId();
            var userName = GetUserFullName();
            if (userId == null) return RedirectToLogin();

            //Creat a post object
            var newComment = new Comment()
            {
                UserId = userId.Value,
                PostId = postCommentVM.PostId,
                Content = postCommentVM.Content,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };

            await _postsService.AddPostCommentAsync(newComment);

            var post = await _postsService.GetPostByIdAsync(postCommentVM.PostId);
            if (userId != post.UserId)
                await _notificationService.AddNewNotificationAsync(post.UserId, NotificationType.Comment, userName, postCommentVM.PostId);

            return PartialView("Home/_Post", post);
        }

        [HttpPost]
        public async Task<IActionResult> AddPostReport(PostReportVM postReportVM)
        {
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null) return RedirectToLogin();

            await _postsService.ReportPostAsync(postReportVM.PostId, loggedInUserId.Value);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePostComment(RemoveCommentVM removeCommentVM)
        {
            await _postsService.RemovePostCommentAsync(removeCommentVM.CommentId);

            var post = await _postsService.GetPostByIdAsync(removeCommentVM.PostId);
            return PartialView("Home/_Post", post);
        }

        [HttpPost]
        public async Task<IActionResult> PostRemove(PostRemoveVM postRemoveVM)
        {

            var postRemoved = await _postsService.RemovePostAsync(postRemoveVM.PostId);
            await _hashtagsService.ProcessHashtagsForRemovedPostAsync(postRemoved.Content);

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Search(string query)
        {
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null) return RedirectToLogin();

            if (string.IsNullOrEmpty(query))
            {
                return RedirectToAction("Index");
            }

            var users = await _context.Users
                .Where(u => u.FullName.Contains(query) && u.Id != loggedInUserId.Value) 
                .ToListAsync();

            return View("SearchResults", users);
        }





    }
}