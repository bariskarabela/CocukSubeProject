using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CocukSubeProject.Entities
{
    [Table("Suspects")]
    public class Suspect
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(11)]
        [MinLength(11)]
        public string Tc { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]

        public DateTime DateOfBirth { get; set; }
        [Required]
        [StringLength(10)]
        public string Gender { get; set; }
        [Required]
        [StringLength(50)]
        public string Nationality { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [Required]
        [StringLength(50)]
        public string District { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan.")]
        public DateTime CatchDate { get; set; }


        [Required]
        [StringLength(250)]
        public string CatchAdress { get; set; }
        [StringLength(11)]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan.")]
        [StringLength(100)]
        public string Crime { get; set; }

    }
}
