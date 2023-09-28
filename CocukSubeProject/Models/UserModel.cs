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
}
