using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class LoginDtos
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }
}