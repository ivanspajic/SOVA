using System.ComponentModel.DataAnnotations;

namespace SOVA.Models
{
    public class UserForCreation
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
