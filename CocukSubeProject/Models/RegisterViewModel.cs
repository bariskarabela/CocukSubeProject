using System.ComponentModel.DataAnnotations;

namespace CocukSubeProject.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Sicil giriniz.")]
        [MaxLength(6, ErrorMessage = "6 hane olmalıdır.")]
        [MinLength(6, ErrorMessage = "6 hane olmalıdır.")]
        public string Sicil { get; set; }


        [Required(ErrorMessage = "İsim Soyisim giriniz.")]
        [MinLength(3, ErrorMessage = "Minimum 3 hane olmalıdır.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Şifre giriniz.")]
        [MinLength(3, ErrorMessage = "Minimum 3 hane olmalıdır.")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Şifre giriniz.")]
        [MinLength(3, ErrorMessage = "Minimum 3 hane olmalıdır.")]
        [Compare(nameof(Password), ErrorMessage = "Şifreler birbiriyle eşleşmiyor.")]
        public string RePassword { get; set; }


        [Required(ErrorMessage = "Soyisim giriniz.")]
        [MinLength(3, ErrorMessage = "Minimum 3 hane olmalıdır.")]
        public string District { get; set; }


    }
}
