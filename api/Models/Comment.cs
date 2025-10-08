using System;
using System.ComponentModel.DataAnnotations;


namespace WebApp_project.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [Required]
        public virtual User User { get; set; } = default!;
        [Required]
        public virtual Post Post { get; set; } = default!;
    }

}
