using System.ComponentModel.DataAnnotations;

namespace CocukSubeProject.Models
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Sicil giriniz.")]
        [MaxLength(6, ErrorMessage = "6 hane olmalıdır.")]
        [MinLength(6, ErrorMessage = "6 hane olmalıdır.")]
        public string Sicil { get; set; }


        [Required(ErrorMessage = "Şifre giriniz.")]
        [MinLength(6, ErrorMessage = "Minimum 6 hane olmalıdır.")]
        [MaxLength(16, ErrorMessage = "Maximum 16 hane olmalıdır.")]
        public string Password { get; set; }
    }
}
