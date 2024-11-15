using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly DataContextDapper _dapper;

        public PostController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [HttpGet("Posts")]
        public IEnumerable<Post> GetPosts()
        {
            string sql = "SELECT * FROM TutorialAppSchema.Posts";

            return _dapper.LoadData<Post>(sql);
        }

        [HttpGet("{postId}")]
        public Post GetPostSingle(int postId)
        {
            string sql = $"SELECT * FROM TutorialAppSchema.Posts WHERE postId = {postId}";

            return _dapper.LoadDataSingle<Post>(sql);
        }

        [HttpGet("PostsByUserId/{userId}")]
        public IEnumerable<Post> GetPostsByUser(int userId)
        {
            string sql = $"SELECT * FROM TutorialAppSchema.Posts WHERE UserId = {userId}";

            return _dapper.LoadData<Post>(sql);
        }

        [HttpGet("postsBySearch/{searchParameter}")]
        public IEnumerable<Post> GetPostsBySearch(string searchParameter)
        {
            string sql = $"SELECT * FROM TutorialAppSchema.Posts WHERE postTitle LIKE '%{searchParameter}%' OR postContent LIKE '%{searchParameter}%'";

            return _dapper.LoadData<Post>(sql);
        }

        [HttpGet("MyPosts")]
        public IEnumerable<Post> GetMyPosts()
        {
            string sql = $"SELECT * FROM TutorialAppSchema.Posts WHERE UserId = {User.FindFirst("UserId")?.Value}";

            return _dapper.LoadData<Post>(sql);
        }

        [HttpPost("CreatePost")]
        public IActionResult AddPost(PostToAddDto postToAdd)
        {
            string sql = $"INSERT INTO TutorialAppSchema.Posts (postTitle, postContent, UserId, PostCreated, PostUpdated) VALUES ('{postToAdd.PostTitle}', '{postToAdd.PostContent}', {User.FindFirst("UserId")?.Value}, GETDATE(), GETDATE())";

            if (_dapper.ExecuteData(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to create post");

        }

        [HttpPut("UpdatePost/{postId}")]
        public IActionResult UpdatePost(PostToEditDto postToUpdate)
        {
            string sql = $"UPDATE TutorialAppSchema.Posts SET postTitle = '{postToUpdate.PostTitle}', postContent = '{postToUpdate.PostContent}', PostUpdated = GETDATE() WHERE PostId = {postToUpdate.PostId} and UserId = {User.FindFirst("UserId")?.Value}";

            if (_dapper.ExecuteData(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to update post");

        }

        [HttpDelete("DeletePost/{postId}")]
        public IActionResult DeletePost(int postId)
        {
            string sql = $"DELETE FROM TutorialAppSchema.Posts WHERE PostId = {postId} and UserId = {User.FindFirst("UserId")?.Value}";

            if (_dapper.ExecuteData(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to delete post");

        }


    }
}