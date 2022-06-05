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
    public class AdminController : ControllerBase
    {

        private readonly SqlUserRepo sqlUserRepo;

        public AdminController(GamerHubDBContext db, JwtAuthenticationManager jwtAuthenticationManager)
        {
            sqlUserRepo = new SqlUserRepo(db, jwtAuthenticationManager);
        }

        [Authorize]
        [HttpPost("Register")]
        public IActionResult Register([FromBody] Admin admin)
        {
            if (!sqlUserRepo.CheckAdminWithEmail(admin.Email))
                return BadRequest("Admin With This Email Already Exists");

            if (sqlUserRepo.CreateAdmin(admin))
                return Ok(admin);

            return BadRequest("Invalid Name Or Email");
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] object content)
        {
            var obj = JsonConvert.DeserializeObject<Admin>(content.ToString());
            AdminClient adminClient = sqlUserRepo.AdminLogin(obj);

            if (adminClient != null)
                return Ok(adminClient);


            return BadRequest("Wrong Password Or Email");
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<Admin> admins = sqlUserRepo.GetAllAdmins();
            if (admins != null)
                return Ok(admins);

            return BadRequest();
        }

        [Authorize]
        [HttpPut("UpdateAdmin")]
        public IActionResult Update(Admin admin)
        {
            if (sqlUserRepo.UpdateAdmin(admin))
                return Ok(admin);

            return BadRequest();
        }

        [Authorize]
        [HttpPost("DeleteAdmin")]
        public IActionResult Delete(object content)
        {
            var obj = JsonConvert.DeserializeObject<int>(content.ToString());
            if (sqlUserRepo.DeleteAdmin(obj))
                return Ok();

            return BadRequest();
        }
    }
}
