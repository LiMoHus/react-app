using Microsoft.AspNetCore.Mvc;
using WebApp_project.Models;
using WebApp_project.DAL;
using Microsoft.AspNetCore.Authorization;
using WebApp_project.ViewsModels;


namespace WebApp_project.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PostAPIController : Controller
{
    private readonly IPostRepository _postRepository;
    private readonly ILogger<PostController> _logger;

    public PostAPIController(IPostRepository postRepository, ILogger<PostController> logger)
    {
        _postRepository = postRepository;
        _logger = logger;
    }

    [HttpGet("postlist")]
    public async Task<IActionResult> PostList()
    {
        var posts = await _postRepository.GetAllPosts();
        if(posts == null)
        {
            _logger.LogError("[PostAPIController] Item list not found while executing _postRepository.GetAllPosts()");
            _logger.LogError("Item list not found");
        }
        var postDtos = posts.Select(post => new PostDto
        {
            PostId = post.PostId,
            UserId = post.UserId,
            Caption = post.Caption,
            ImageUrl = post.ImageUrl,
            CreatedAt = post.CreatedAt
        
        });
        return Ok(postDtos);

    }
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] PostDto postDto, IFormFile ImageUrl)
    {
        if (postDto == null)
        {
            return BadRequest("Posts cannot be null");
        }
        var newPost = new Post
        {   
            PostId = postDto.PostId,
            UserId = postDto.UserId,
            Caption = postDto.Caption,
            ImageUrl = postDto.ImageUrl,
            CreatedAt = postDto.CreatedAt

        };
        bool returnOK = await _postRepository.CreatePost(newPost, ImageUrl);
        if(returnOK)
           return CreatedAtAction(nameof(PostList), new {id = newPost.PostId}, newPost);

        _logger.LogWarning("[PostApiController] Post creation failed {@post}", newPost);
        return StatusCode(500, "internal server error");
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPost(int id)
    {
        var post = await _postRepository.GetPostById(id);
        if (post == null)
        {
            _logger.LogError("[PostAPIController] post not found for the PostId {PostId:0000}", id);
            return NotFound("Post not found for the ItemId");
        }
        return Ok(post);
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PostDto postDto, IFormFile ImageUrl)
    {
        if (postDto == null)
        {
            return BadRequest("Post data cannot be null");
        }
        // Find the post in the database
        var existingPost = await _postRepository.GetPostById(id);
        if (existingPost == null)
        {
            return NotFound("Post not found");
        }
        // Update the Post properties
        existingPost.PostId = postDto.PostId;
        existingPost.UserId = postDto.UserId;
        existingPost.Caption = postDto.Caption;
        existingPost.ImageUrl = postDto.ImageUrl;
        existingPost.CreatedAt = postDto.CreatedAt;
        // Save the changes
        bool updateSuccessful = await _postRepository.UpdatePost(existingPost, ImageUrl);
        if (updateSuccessful)
        {
            return Ok(existingPost); // Return the updated item
        }

        _logger.LogWarning("[PostAPIController] Post update failed {@post}", existingPost);
        return StatusCode(500, "Internal server error");
    }




}
public class PostController : Controller
{
    private readonly IPostRepository _postRepository;
    private readonly ILogger<PostController> _logger;

    public PostController(IPostRepository postRepository,ILogger<PostController> logger )
    {
        _postRepository = postRepository;
        _logger = logger;
    }
    

    public async Task<IActionResult> Page()
    {
        
        var posts = await _postRepository.GetAllPosts();
        if(posts == null){
            _logger.LogWarning("[PostController] post list not found while executing _PostRepository.GetAll()");
            return NotFound("post list not found");
        }
        return View(posts);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Post post, IFormFile ImageUrl)
    {  
        bool returnOk = await _postRepository.CreatePost(post,ImageUrl);
        if (returnOk){
            return RedirectToAction(nameof(Page));
        }
        else{
            _logger.LogError("[PostController] post deletion failed for the PostId {returnOk}", returnOk);
            return RedirectToAction(nameof(Page));
        }
    
    }


    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        bool returnOk = await _postRepository.DeletePost(id);
        if (!returnOk)
        {   
            _logger.LogError("[PostController] post deletion failed for the PostId {PostId:0000}", id);
            return BadRequest("Post deletion failed");
        }
        return RedirectToAction(nameof(Page));
    }


    [HttpGet]
    [Authorize]
    public async Task<IActionResult> UpdatePosts(int id)
    {
        var post = await _postRepository.GetPostById(id);
        if (post == null)
        {
            _logger.LogError("[PostController] post not found when updating the PostId {PostId:0000}", id);
            return BadRequest("Post not found for the PostId");
        }

        return PartialView("_ModalPopup",new AddPostViewModel(null,post));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UpdatePosts(Post post, IFormFile ImageUrl)
    {

            bool returnOk = await _postRepository.UpdatePost(post,ImageUrl);
            if (returnOk){
                _logger.LogWarning("[PostController] this is the image i got {post} ", ImageUrl);
                return RedirectToAction(nameof(Page)); 
            }         
        else{
            _logger.LogWarning("[PostController] post update failed {@post}", post);
        return RedirectToAction(nameof(Page));
        }

         
    }


   
}
