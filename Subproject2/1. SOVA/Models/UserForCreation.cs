using System.ComponentModel.DataAnnotations;

namespace _1._SOVA.Models
{
    public class UserForCreation
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
