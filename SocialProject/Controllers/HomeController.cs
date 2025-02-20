using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialProject.Data;
using System;
using System.Diagnostics;

namespace SocialProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SocialMediaContext _context;

        public HomeController(ILogger<HomeController> logger, SocialMediaContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var allPosts = await _context.Posts
                .Include(n => n.User)
                .ToListAsync();

            return View();
        }

     

    }
}
