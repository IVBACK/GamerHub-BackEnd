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
    public class FriendshipController : ControllerBase
    {

        private readonly SqlUserRepo sqlUserRepo;

        public FriendshipController(GamerHubDBContext db, JwtAuthenticationManager jwtAuthenticationManager)
        {
            sqlUserRepo = new SqlUserRepo(db, jwtAuthenticationManager);
        }

        [Authorize]
        [HttpPost("CreateFriendRequest")]
        public IActionResult CreateFriendRequest([FromBody] object content)
        {
            var obj = JsonConvert.DeserializeObject<FriendRequest>(content.ToString());

            User requestedBy = sqlUserRepo.GetUserById(obj.RequestedById);
            User requestedTo = sqlUserRepo.GetUserById(obj.RequestedToId);

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

        [Authorize]
        [HttpPost("AcceptFriendRequest")]
        public IActionResult AcceptFriendRequest([FromBody] object content)
        {
            var obj = JsonConvert.DeserializeObject<FriendRequest>(content.ToString());

            User requestedBy = sqlUserRepo.GetUserById(obj.RequestedById);
            User requestedTo = sqlUserRepo.GetUserById(obj.RequestedToId);

            if (requestedBy != null && requestedTo != null)
            {
                var friendship = requestedBy.SentFriendships
                    .FirstOrDefault(x => x.RequestedById == requestedBy.Id);
                friendship.FriendRequestFlag = FriendRequestFlag.Approved;

                var friendshipp = requestedTo.ReceivedFriendships
                    .FirstOrDefault(x => x.RequestedToId == requestedTo.Id);
                friendshipp.FriendRequestFlag = FriendRequestFlag.Approved;

                sqlUserRepo.UpdateUser(requestedBy);
                sqlUserRepo.UpdateUser(requestedTo);
                return Ok();
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPost("RejectFriendRequest")]
        public IActionResult RejectFriendRequest(object content)
        {
            var obj = JsonConvert.DeserializeObject<FriendRequest>(content.ToString());

            User requestedBy = sqlUserRepo.GetUserById(obj.RequestedById);
            User requestedTo = sqlUserRepo.GetUserById(obj.RequestedToId);

            if (requestedBy != null && requestedTo != null)
            {
                var friendship = requestedBy.ReceivedFriendships
                    .Single(x => x.RequestedById == requestedBy.Id);
                friendship.FriendRequestFlag = FriendRequestFlag.Rejected;

                var friendshipp = requestedTo.SentFriendships
                    .FirstOrDefault(x => x.RequestedToId == requestedTo.Id);
                friendshipp.FriendRequestFlag = FriendRequestFlag.Rejected;

                sqlUserRepo.UpdateUser(requestedBy);
                sqlUserRepo.UpdateUser(requestedTo);
                return Ok();
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPost("GetReceivedFriendships")]
        public IActionResult GetReceivedFriendships([FromBody] Search search)
        {
            User user = sqlUserRepo.GetUserById(int.Parse(search.SearchString));

            if (user != null)
            {
                var receivedfriendship = user.ReceivedFriendships.Where(p => p.FriendRequestFlag == FriendRequestFlag.Waiting);
                var friendRequests = new List<Friend>();

                foreach (var friendShip in receivedfriendship)
                {
                    User userr = sqlUserRepo.GetOnlyUserById(friendShip.RequestedById);
                    Friend friendd = new Friend()
                    {
                        Id = userr.Id,
                        Name = userr.Name,
                        Email = userr.Email,
                        Gender = userr.Gender,
                        BirthDate = userr.BirthDate
                    };
                    friendRequests.Add(friendd);
                }

                return Ok(friendRequests);
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPost("GetFriends")]
        public IActionResult GetFriends([FromBody] Search search)
        {
            User user = sqlUserRepo.GetUserById(int.Parse(search.SearchString));

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
                        Email = x.Email,
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
    }
}
