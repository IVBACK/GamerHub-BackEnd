using System.ComponentModel.DataAnnotations.Schema;

namespace GamerHub.CORE.Models
{

    public class Friendship
    {
        [ForeignKey("RequestedById")]
        public int RequestedById { get; set; }

        [ForeignKey("RequestedToId")]
        public int RequestedToId { get; set; }

        public User RequestedBy { get; set; }
        public User RequestedTo { get; set; }


        public DateTime RequestTime { get; set; }

        public FriendRequestFlag? FriendRequestFlag { get; set; }

    }
    public enum FriendRequestFlag
    {
        Waiting,
        Approved,
        Rejected,
        Blocked,
        Spam
    };
}
