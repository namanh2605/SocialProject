using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialProject.Data;
using System;

namespace SocialProject.ViewComponents
{
    public class HashtagsViewComponent : ViewComponent
    {
        private readonly SocialMediaContext _context;
        public HashtagsViewComponent(SocialMediaContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var oneWeekAgoNow = DateTime.UtcNow.AddDays(-7);

            var top3Hashtags = await _context.Hashtags
                .Where(h => h.DateCreated >= oneWeekAgoNow)
                .OrderByDescending(n => n.Count)
                .Take(3)
                .ToListAsync();

            return View(top3Hashtags);
        }
    }
}