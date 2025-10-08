using WebApp_project.Models;

namespace WebApp_project.ViewsModels
{
    public class CommentViewModel
    {
        public int PostId;
        public Comment Comment;
        public CommentViewModel(int postId, Comment comment)
        {
            PostId = postId;
            Comment= comment;
        }
        
    }
}