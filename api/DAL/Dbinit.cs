using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp_project.Models;

namespace WebApp_project.DAL
{
    public static class DBInit
    {
        public static void Seed(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<PostDbContext>();
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            // Seed Identity Roles if not already present


            // Seed Identity Users
            SeedUsers(userManager);

            // Ensure the posts and comments are seeded (you already have this)
            SeedPostsAndComments(context);
        }

     
        private static void SeedUsers(UserManager<User> user)
        {
                var user1 = new User
                {
                    UserName = "admin1@admin.com",
                    Email = "admin1@admin.com",
                    FullName= "Ahmed Mohamed"
                };

                user.CreateAsync(user1, "Password123!");
                var user2 = new User
                {
                    UserName = "admin2@admin.com",
                    Email = "admin2@admin.com",
                    FullName= "Musdafa Mohamed"
                };

                user.CreateAsync(user2, "Password1234!");

                var user3 = new User
                {
                    UserName = "admin3@admin.com",
                    Email = "admin3@admin.com",
                    FullName= "Abdiasis Mohamed"
                };

                user.CreateAsync(user3, "Password12345!");

            }



        private static void SeedPostsAndComments(PostDbContext context)
        {
            if (!context.Posts.Any())
            {
                var posts = new List<Post>
                {
                    new Post { UserId = 1, Caption = "our somali queen😍", ImageUrl = "/images/image.png" },
                    new Post { UserId = 2, Caption = "welcome to our culture day", ImageUrl = "/images/culture.jpg" },
                    new Post { UserId = 3, Caption = "i ate too much today!!!", ImageUrl = null }, // Use null for missing image
                };
                context.AddRange(posts);
                context.SaveChanges();
            }

            if (!context.Comments.Any())
            {
                var comments = new List<Comment>
                {
                    new Comment {UserId = 1, PostId = 2, Content = "FOR THE CULTURE"},
                    new Comment {UserId = 1, PostId = 3, Content = "don't get too fat🤣"},
                    new Comment {UserId = 2, PostId = 1, Content = "our woman😍"},
                    new Comment {UserId = 2, PostId = 3, Content = "i wanna come"},
                    new Comment {UserId = 3, PostId = 1, Content = "my wife"},
                    new Comment {UserId = 3, PostId = 2, Content = "FOR THE CULTURE"},
                };
                context.AddRange(comments);
                context.SaveChanges();
            }
        }
    
    }
}

