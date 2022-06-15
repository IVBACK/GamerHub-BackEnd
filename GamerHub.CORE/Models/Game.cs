using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamerHub.CORE.Models
{
    public class Game
    {
        [Key]
        public int? GameId { get; set; }

        [Required]
        [StringLength(50)]
        public string GameName { get; set; }

        [Required]
        [StringLength(50)]
        public GameGenre GameGenre { get; set; }
    }

    public enum GameGenre
    {
        Action,
        ActionAdventure,
        Adventure,
        RolePlaying,
        Simulation,
        Strategy,
        Sports,
        Puzzle
    }
}
