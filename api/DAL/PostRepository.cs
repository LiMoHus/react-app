using Microsoft.EntityFrameworkCore;
using WebApp_project.Models;
using Microsoft.AspNetCore.Identity;



namespace WebApp_project.DAL;
    public class PostRepository : IPostRepository
    {
        private readonly PostDbContext _db;
        private readonly ILogger<PostRepository> _logger;
        private readonly UserManager<User> _userManager;
        
        public PostRepository(UserManager<User> userManager, PostDbContext db, ILogger<PostRepository> logger)
        {
            _db = db;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Post>?> GetAllPosts()
        {
            try
            {
                return await _db.Posts
                    .OrderByDescending(p => p.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while retrieving posts.");
                return null;
            }
        }

        public async Task<IEnumerable<Comment>?> GetAllComments()
        {
            try
            {
                return await _db.Comments
                    .OrderByDescending(c => c.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while retrieving comments.");
                return null;
            }
        }

        public async Task<Post?> GetPostById(int id)
            {
                try
                {
                    return await _db.Posts.FindAsync(id);
                }
                catch (Exception e)
                {
                    _logger.LogError("[PostRepository] post FindAsync(id) failed when GetPostById for PostId {PostId:0000}, error message: {e}", id, e.Message);
                    return null;
                }
            }
       
        public async Task<bool> CreatePost(Post post, IFormFile ImageUrl)
        {
            
            try
            {
                if (ImageUrl != null && ImageUrl.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(ImageUrl.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        _logger.LogError("Invalid image format. Allowed formats: .jpg, .jpeg, .png, .gif");
                        return false;
                    }

                    var uploadsFolder = Path.Combine("wwwroot", "Images");

                    var filePath = Path.Combine(uploadsFolder, ImageUrl.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageUrl.CopyToAsync(stream);
                    }
                    post.ImageUrl = $"/Images/{ImageUrl.FileName}";
                }
                await _db.Posts.AddAsync(post);
                await _db.SaveChangesAsync();
                return true;

            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while creating a post.");
                return false;
            }
        }

        public async Task<bool> CreateComment(Comment comment)
        {
            try
            {
                await _db.Comments.AddAsync(comment);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while creating a comment.");
                return false;
            }
        }

        public async Task<bool> UpdatePost(Post post, IFormFile ImageUrl)
        {
            try
            {
                if (ImageUrl != null && ImageUrl.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(ImageUrl.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        _logger.LogError("Invalid image format. Allowed formats: .jpg, .jpeg, .png, .gif");
                        return false;
                    }

                    var uploadsFolder = Path.Combine("wwwroot", "Images");

                    var filePath = Path.Combine(uploadsFolder, ImageUrl.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageUrl.CopyToAsync(stream);
                    }
                    post.ImageUrl = $"/Images/{ImageUrl.FileName}";
                }
                    _db.Posts.Update(post);
                    await _db.SaveChangesAsync();
                    return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while updating a post.");
                return false;
            }
        }

        public async Task<bool> DeletePost(int id)
        {
            try
            {
                var post = await _db.Posts.FindAsync(id);
                if (post == null)
                {
                    return false;
                }

                _db.Posts.Remove(post);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occurred while deleting the post with ID {id}.");
                return false;
            }
        }

        public async Task<bool> DeleteComment(int id)
        {
            try
            {
                var comment = await _db.Comments.FindAsync(id);
                if (comment == null)
                {
                    return false;
                }

                _db.Comments.Remove(comment);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e , $"An error occurred while deleting the comment with ID {id}.");
                return false;
            }
        }
    }