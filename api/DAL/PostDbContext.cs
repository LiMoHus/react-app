using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp_project.Models;

namespace WebApp_project.DAL;

public class PostDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
	public PostDbContext(DbContextOptions<PostDbContext> options) : base(options)
	{
       // Database.EnsureCreated();
	}

	public DbSet<Post> Posts { get; set; }
	
	public DbSet<Comment> Comments { get; set; }
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }

	
}
