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
    //[Authorize]
    public class FriendshipController : ControllerBase
    {

        private readonly SqlUserRepo sqlUserRepo;

        public FriendshipController(GamerHubDBContext db, JwtAuthenticationManager jwtAuthenticationManager)
        {
            sqlUserRepo = new SqlUserRepo(db, jwtAuthenticationManager);
        }

        //[Authorize]
        [HttpPost("CreateFriendRequest")]
        public IActionResult CreateFriendRequest(int i, int y)
        {
            User requestedBy = sqlUserRepo.GetUserById(i);
            User requestedTo = sqlUserRepo.GetUserById(y);

            if (requestedBy != null && requestedTo != null)
            {
                Friendship friendship = new Friendship
                {
                    FriendRequestFlag = FriendRequestFlag.Waiting,
                    RequestedBy = requestedBy,
                    RequestedById = (int)requestedBy.Id,
                    RequestedTo = requestedTo,
                    RequestedToId = (int)requestedTo.Id,
                    RequestTime = DateTime.Now
                };

                requestedBy.SentFriendships.Add(friendship);
                requestedTo.ReceivedFriendships.Add(friendship);

                sqlUserRepo.UpdateUser(requestedBy);
                sqlUserRepo.UpdateUser(requestedTo);
                return Ok();
            }
            return BadRequest();
        }

        //[Authorize]
        [HttpPost("AcceptFriendRequest")]
        public IActionResult AcceptFriendRequest(int i, int y)
        {
            User requestedBy = sqlUserRepo.GetUserById(i);
            User requestedTo = sqlUserRepo.GetUserById(y);

            if (requestedBy != null && requestedTo != null)
            {
                var friendship = requestedBy.SentFriendships
                    .FirstOrDefault(x => x.RequestedById == i);
                friendship.FriendRequestFlag = FriendRequestFlag.Approved;

                var friendshipp = requestedTo.ReceivedFriendships
                    .FirstOrDefault(x => x.RequestedToId == y);
                friendshipp.FriendRequestFlag = FriendRequestFlag.Approved;

                sqlUserRepo.UpdateUser(requestedBy);
                sqlUserRepo.UpdateUser(requestedTo);
                return Ok();
            }
            return BadRequest();
        }

        //[Authorize]
        [HttpPost("RejectFriendRequest")]
        public IActionResult RejectFriendRequest(int i, int y)
        {
            User requestedBy = sqlUserRepo.GetUserById(i);
            User requestedTo = sqlUserRepo.GetUserById(y);

            if (requestedBy != null && requestedTo != null)
            {
                var friendship = requestedBy.ReceivedFriendships
                    .Single(x => x.RequestedById == y);
                friendship.FriendRequestFlag = FriendRequestFlag.Rejected;

                var friendshipp = requestedTo.SentFriendships
                    .FirstOrDefault(x => x.RequestedToId == i);
                friendshipp.FriendRequestFlag = FriendRequestFlag.Rejected;

                sqlUserRepo.UpdateUser(requestedBy);
                sqlUserRepo.UpdateUser(requestedTo);
                return Ok();
            }
            return BadRequest();
        }

        //[Authorize]
        [HttpPost("GetFriends")]
        public IActionResult GetFriends([FromBody] object content)
        {
            var obj = JsonConvert.DeserializeObject<int>(content.ToString());
            User user = sqlUserRepo.GetUserById(obj);

            if (user != null)
            {
                var receivedfriendship = user.ReceivedFriendships.Where(p => p.FriendRequestFlag == FriendRequestFlag.Approved);
                var sentfriendship = user.SentFriendships.Where(p => p.FriendRequestFlag == FriendRequestFlag.Approved);

                var friends = new List<Friend>();

                foreach (var friend in receivedfriendship)
                {
                    var x = sqlUserRepo.GetUserById(friend.RequestedById);
                    Friend friendd = new Friend()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Email= x.Email,
                        Gender = x.Gender,
                        BirthDate = x.BirthDate,
                    };
                    friends.Add(friendd);
                }

                foreach (var friend in sentfriendship)
                {
                    var x = sqlUserRepo.GetUserById(friend.RequestedToId);
                    Friend friendd = new Friend()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Email = x.Email,
                        Gender = x.Gender,
                        BirthDate = x.BirthDate,
                    };
                    friends.Add(friendd);
                }

                return Ok(friends);
            }
            return BadRequest();
        }

        [HttpPost("GetFriendships")]
        public IActionResult GetFriendships([FromBody] object content)
        {
            var obj = JsonConvert.DeserializeObject<int>(content.ToString());
            User user = sqlUserRepo.GetUserById(obj);

            if (user != null)
            {
                var receivedfriendship = user.ReceivedFriendships;
                var sentfriendship = user.SentFriendships;

                var friends = new List<String>();

                foreach (var friend in receivedfriendship)
                {
                    friends.Add(sqlUserRepo.GetUserById(friend.RequestedById).Name);
                }

                foreach (var friend in sentfriendship)
                {
                    friends.Add(sqlUserRepo.GetUserById(friend.RequestedToId).Name);
                }

                return Ok(friends);
            }
            return BadRequest();
        }
    }
}
