using System.ComponentModel.DataAnnotations;

namespace CocukSubeProject.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Sicil { get; set; }
        public string FullName { get; set; }

        public string District { get; set; }
        public bool Locked { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Role { get; set; } = "user";
        public string? ProfileImageFileName { get; set; } = "no-image.jpg";
    }
    public class EditUserModel
    {
        [Required(ErrorMessage = "Sicil giriniz.")]
        [MaxLength(6, ErrorMessage = "6 hane olmalıdır.")]
        [MinLength(6, ErrorMessage = "6 hane olmalıdır.")]
        public string Sicil { get; set; }


        [Required(ErrorMessage = "İsim Soyisim giriniz.")]
        [MinLength(2, ErrorMessage = "Minimum 2 hane olmalıdır.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Birim giriniz.")]
        [MinLength(3, ErrorMessage = "Minimum 3 hane olmalıdır.")]
        public string District { get; set; }


        public string Role { get; set; } = "user";
        public bool Locked { get; set; } = false;
    }
}
