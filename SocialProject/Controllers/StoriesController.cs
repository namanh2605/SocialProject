using SocialProject.Data;
using SocialProject.Data.Models;
using SocialProject.ViewModals.Stories;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.EntityFrameworkCore;

namespace SocialProject.Controllers
{
    public class StoriesController : Controller
    {
        private readonly SocialMediaContext _context;
        public StoriesController(SocialMediaContext context)
        {
            _context = context;
        }

     
        [HttpPost]
        public async Task<IActionResult> CreateStory(StoryVM storyVM)
        {
            int loggedInUserId = 1;

            var newStory = new Story
            {
                DateCreated = DateTime.UtcNow,
                IsDeleted = false,
                UserId = loggedInUserId
            };

            if (storyVM.Image != null && storyVM.Image.Length > 0)
            {
                string rootFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                if (storyVM.Image.ContentType.Contains("image"))
                {
                    string rootFolderPathImages = Path.Combine(rootFolderPath, "images/stories");
                    Directory.CreateDirectory(rootFolderPathImages);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(storyVM.Image.FileName);
                    string filePath = Path.Combine(rootFolderPathImages, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await storyVM.Image.CopyToAsync(stream);

                    newStory.ImageUrl = "/images/stories/" + fileName;
                }
            }

            await _context.Stories.AddAsync(newStory);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}