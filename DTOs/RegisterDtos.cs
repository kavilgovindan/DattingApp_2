using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDtos
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }
}