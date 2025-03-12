using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialProject.Data;
using System;

namespace SocialProject.ViewComponents
{
    public class StoriesViewComponent : ViewComponent
    {
        private readonly SocialMediaContext _context;
        public StoriesViewComponent(SocialMediaContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var allStories = await _context.Stories
                .Where(n => n.DateCreated >= DateTime.UtcNow.AddHours(-24))
                .Include(s => s.User)
                .ToListAsync();

            return View(allStories);
        }
    }
}