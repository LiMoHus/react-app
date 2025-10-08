using Microsoft.AspNetCore.Mvc;
using WebApp_project.Models;
using WebApp_project.DAL;

namespace WebApp_project.Controllers;

public class CommentController : Controller
{
    private readonly IPostRepository _postRepository;
    private readonly ILogger<CommentController> _logger;

    public CommentController(IPostRepository postRepository, ILogger<CommentController> logger)
    {
        _postRepository = postRepository;
        _logger = logger;
    }
    [HttpPost]
    public async Task<IActionResult> AddComment(Comment comment)
    {   
        bool returnOk = await _postRepository.CreateComment(comment);
        if (returnOk){
            return RedirectToAction("Page", "Post");
        }      
        else
        {
            _logger.LogWarning("[CommentController] comment creation failed {@comment}", comment);
            return BadRequest("comment creation failed");
        }

    }

    /*[HttpPost]
     public async Task<IActionResult> Update(Comment comment)
    {
      
        bool returnOk = await _postRepository.UpdateComment(comment);
        if (returnOk){
            return RedirectToAction("Page", "Post");
        }
        else{
            _logger.LogWarning("[CommentController] comment updating failed {@comment}", comment);
            return  BadRequest("comment update failed");
        }
       
    }*/

    [HttpPost]
     public async Task<IActionResult> Delete(int id)
    {
        bool returnOk = await _postRepository.DeleteComment(id);
        if (!returnOk)
            {
                _logger.LogWarning("[CommentController] post creation failed {@comment}", id);
                return BadRequest("comment deletion failed");
            }
        return RedirectToAction("Page","Post");
    }
    
}
