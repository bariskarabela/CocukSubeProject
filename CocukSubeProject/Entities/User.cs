using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CocukSubeProject.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(11)]
        [MaxLength(6)]
        [MinLength(6)]
        public string Sicil { get; set; }
        [Required]
        [StringLength(100)]
        [MinLength(6)]
        public string Password { get; set; }
        [Required]
        [StringLength(50)]
        [MinLength(3)]
        public string FullName { get; set; }

        [Required]
        [StringLength(50)]
        public string District { get; set; }
        public bool Locked { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [StringLength(10)]
        [Required]
        public string Role { get; set; } = "user";
        [StringLength(255)]
        public string? ProfileImageFileName { get; set; } = "no-image.jpg";

    }
}
