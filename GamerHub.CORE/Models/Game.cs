using System.ComponentModel.DataAnnotations;

namespace GamerHub.CORE.Models
{
    public class Game
    {
        [Key]
        public int? GameId { get; set; }

        [Required]
        [StringLength(50)]
        public string GameName { get; set; }
    }
}
