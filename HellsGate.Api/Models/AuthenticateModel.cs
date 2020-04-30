using System.ComponentModel.DataAnnotations;

namespace HellsGate.Models
{
    public class AuthenticateModel
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string Username { get; set; }
    }
}