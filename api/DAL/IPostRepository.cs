using WebApp_project.Models;
using WebApp_project.ViewsModels;

namespace WebApp_project.DAL;

public interface IPostRepository
{
	Task<IEnumerable<Post>?> GetAllPosts();
    Task<IEnumerable<Comment>?> GetAllComments();
	Task<bool> CreatePost(Post post,IFormFile Imageurl);
    Task<Post?> GetPostById(int id);
    Task<bool> CreateComment(Comment comment);
    Task<bool> UpdatePost(Post post,IFormFile Imageurl);
    Task<bool> DeletePost(int id);
    Task<bool> DeleteComment(int id);
}
