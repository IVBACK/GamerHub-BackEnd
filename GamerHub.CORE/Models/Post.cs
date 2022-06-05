using System.ComponentModel.DataAnnotations;

namespace GamerHub.CORE.Models
{
    public class Post
    {
        [Key]
        public int? PostId { get; set; }

        [Required]
        [StringLength(150)]
        public string Image { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; }

        [Required]
        [StringLength(700)]
        public string Content { get; set; }

        [Required]
        [StringLength(50)]
        public string GameName { get; set; }
    }
}
