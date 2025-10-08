
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;
using Microsoft.AspNetCore.Identity;


namespace WebApp_project.Models
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; } = string.Empty;
    
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        
        public virtual List<Post> Posts { get; set; } = new List<Post>();

        public virtual List<Comment> Comments { get; set; } = new List<Comment>();
        
        // Self-referencing many-to-many relationship for followers
    }
}
