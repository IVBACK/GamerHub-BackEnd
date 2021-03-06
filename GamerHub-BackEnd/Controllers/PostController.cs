using GamerHub.CORE.Models;
using GamerHub.CORE.WrapperModels;
using GamerHub.DATA.DBContext;
using GamerHub.SERVICE.SqlRepos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GamerHub_BackEnd.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly SqlPostRepo sqlPostRepo;

        public PostController(GamerHubDBContext db)
        {
            sqlPostRepo = new SqlPostRepo(db);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<Post> posts = sqlPostRepo.GetAllPosts();
            if (posts != null)
                return Ok(posts);

            return BadRequest();
        }

        [Authorize]
        [HttpPost("Search")]
        public IActionResult Search([FromBody] Search search)
        {
            if (!string.IsNullOrEmpty(search.SearchString))
            {
                IEnumerable<Post> postsFiltered = sqlPostRepo.SearchPost(search.SearchString);
                return Ok(postsFiltered);
            }
            else
                return Ok();
        }

        [Authorize]
        [HttpPost("Createpost")]
        public IActionResult Create([FromBody] Post post)
        {
            if (sqlPostRepo.CreatePost(post))
                return Ok();

            return BadRequest();
        }

        [Authorize]
        [HttpPut("UpdatePost")]
        public IActionResult Update([FromBody] object content)
        {
            var obj = JsonConvert.DeserializeObject<Post>(content.ToString());
            if (sqlPostRepo.UpdatePost(obj))
                return Ok(obj);

            return BadRequest();
        }

        [Authorize]
        [HttpPost("DeletePost")]
        public IActionResult Delete([FromBody] object content)
        {
            var obj = JsonConvert.DeserializeObject<int>(content.ToString());
            if (sqlPostRepo.DeletePost(obj))
                return Ok();

            return BadRequest();
        }
    }
}
