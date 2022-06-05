using System.ComponentModel.DataAnnotations;

namespace GamerHub.CORE.Models
{
    public class Admin
    {
        [Key]
        public int? Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        [StringLength(20)]
        public string Gender { get; set; }

        [Required]
        [StringLength(50)]
        public string BirthDate { get; set; }
    }
}
