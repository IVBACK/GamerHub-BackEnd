using GamerHub.CORE.Models;
using GamerHub.CORE.WrapperModels;
using GamerHub.DATA.DBContext;
using GamerHub.SERVICE.Security;
using GamerHub.SERVICE.SqlRepos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GamerHub_BackEnd.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly SqlUserRepo sqlUserRepo;

        public UsersController(GamerHubDBContext db, JwtAuthenticationManager jwtAuthenticationManager)
        {
            sqlUserRepo = new SqlUserRepo(db, jwtAuthenticationManager);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register([FromBody] User user)
        {
            if (!sqlUserRepo.CheckUserWithEmail(user.Email))
                return BadRequest("Account With This Email Already Exists");


            if (sqlUserRepo.CreateUser(user))
                return Ok();

            return BadRequest("Invalid Name Or Email");
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] object content)
        {
            var obj = JsonConvert.DeserializeObject<User>(content.ToString());
            UserClient userClient = sqlUserRepo.UserLogin(obj);

            if (userClient != null)
                return Ok(userClient);


            return BadRequest("Wrong Password Or Email");
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<User> users = sqlUserRepo.GetAllUsers();
            if (users != null)
                return Ok(users);

            return BadRequest();
        }

        [Authorize]
        [HttpPost("Search")]
        public IActionResult Search([FromBody] object content)
        {
            var obj = JsonConvert.DeserializeObject<string>(content.ToString());
            IEnumerable<User> usersFiltered = sqlUserRepo.SearchUser(obj);
            if (usersFiltered != null)
                return Ok(usersFiltered);

            return BadRequest();
        }

        [Authorize]
        [HttpPost("SearchById")]
        public IActionResult SearchById([FromBody] object content)
        {
            var obj = JsonConvert.DeserializeObject<int>(content.ToString());
            User user = sqlUserRepo.GetUserById(obj);
            if (user != null)
                return Ok(user);

            return BadRequest();
        }

        [Authorize]
        [HttpPut("UpdateUser")]
        public IActionResult Update([FromBody] User user)
        {
            if (sqlUserRepo.UpdateUser(user))
                return Ok(user);

            return BadRequest();
        }

        [Authorize]
        [HttpPost("DeleteUser")]
        public IActionResult Delete([FromBody] object content)
        {
            var obj = JsonConvert.DeserializeObject<int>(content.ToString());
            if (sqlUserRepo.DeleteUser(obj))
                return Ok();

            return BadRequest();
        }
    }
}
