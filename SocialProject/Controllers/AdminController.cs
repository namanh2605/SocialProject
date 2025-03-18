using Microsoft.AspNetCore.Mvc;
using SocialProject.Data;
using System;

namespace SocialProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly SocialMediaContext _context;
        public AdminController(SocialMediaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var allPosts = await _context.Posts.Include(n => n.User).ToListAsync();

            return View(allPosts);
        }
    }
}