using SocialProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialProject.Data.Helpers
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(SocialMediaContext socialMediaContext)
        {
            if (!socialMediaContext.Users.Any() && !socialMediaContext.Posts.Any())
            {
                var newUser = new User()
                {
                    FullName = "Cristiano Ronaldo\r\n",
                    ProfilePictureUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTFc0Cry8E_MF-5Qkl5umnXnZ77LI0B8tYKTn-nIG48KTFKnzxLHhIP2Usqb8Hsq0ERpH8_pM0M06a1kB-A0CToMw"
                };
                await socialMediaContext.Users.AddAsync(newUser);
                await socialMediaContext.SaveChangesAsync();

                var newPostWithoutImage = new Post()
                {
                    Content = "This is going to be our first post which is being loaded from the database and it has been created using our test user.",
                    ImageUrl = "",
                    NrOfReports = 0,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,

                    UserId = newUser.Id
                };

                var newPostWithImage = new Post()
                {
                    Content = "This is going to be our first post which is being loaded from the database and it has been created using our test user. This post has an image",
                    ImageUrl = "https://unsplash.com/photos/foggy-mountain-summit-1Z2niiBPg5A",
                    NrOfReports = 0,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,

                    UserId = newUser.Id
                };

                await socialMediaContext.Posts.AddRangeAsync(newPostWithoutImage, newPostWithImage);
                await socialMediaContext.SaveChangesAsync();
            }
        }
    }
}