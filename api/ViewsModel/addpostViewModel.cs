using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using WebApp_project.Models;


namespace WebApp_project.ViewsModels
{
    public class AddPostViewModel
    {
        [Required(ErrorMessage = "An image file is required.")]
        
        public IFormFile? ImageUrl;
        public Post Post;

        public AddPostViewModel(IFormFile? imageFile, Post post)
        {
            ImageUrl = imageFile;
            Post = post;
        }
    }
}
