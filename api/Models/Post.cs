using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp_project.Models
{

    public class Post
    {


        public int PostId { get; set; }
        [Required]
        public int UserId { get; set; }

        [MinLength(5, ErrorMessage = "Caption must be at least 5 characters long.")]
        [StringLength(300, ErrorMessage = "Caption cannot exceed 300 characters.")]
        public string? Caption { get; set; }

        public  string? ImageUrl { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public virtual User User { get; set; } = default!;  
        public virtual List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
