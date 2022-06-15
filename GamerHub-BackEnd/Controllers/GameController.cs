using GamerHub.CORE.Models;
using GamerHub.CORE.WrapperModels;
using GamerHub.DATA.DBContext;
using GamerHub.SERVICE.SqlRepos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace GamerHub_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GameController : ControllerBase
    {
        private readonly SqlGameRepo sqlGameRepo;

        public GameController(GamerHubDBContext db)
        {
            sqlGameRepo = new SqlGameRepo(db);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<Game> games = sqlGameRepo.GetAllGames();
            if (games != null)
                return Ok(games);

            return BadRequest();
        }

        [Authorize]
        [HttpPost("Search")]
        public IActionResult Search([FromBody] Search search)
        {
            if (!string.IsNullOrEmpty(search.SearchString))
            {
                IEnumerable<Game> postsFiltered = sqlGameRepo.SearchGame(search.SearchString);
                return Ok(postsFiltered);
            }
            else
                return Ok();
        }

        [Authorize]
        [HttpPost("CreateGame")]
        public IActionResult Create([FromBody] Game game)
        {
            if (sqlGameRepo.CreateGame(game))
                return Ok();

            return BadRequest();
        }

        [Authorize]
        [HttpPut("UpdateGame")]
        public IActionResult Update([FromBody] object content)
        {
            var obj = JsonConvert.DeserializeObject<Game>(content.ToString());
            if (sqlGameRepo.UpdateGame(obj))
                return Ok(obj);

            return BadRequest();
        }

        [Authorize]
        [HttpPost("DeleteGame")]
        public IActionResult Delete([FromBody] object content)
        {
            var obj = JsonConvert.DeserializeObject<int>(content.ToString());
            if (sqlGameRepo.DeleteGame(obj))
                return Ok();

            return BadRequest();
        }
    }
}
