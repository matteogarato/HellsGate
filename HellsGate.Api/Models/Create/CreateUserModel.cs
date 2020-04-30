using System.ComponentModel.DataAnnotations;

namespace HellsGate.Api.Models.Create
{
    public class CreateUserModel
    {
        [Required]
        public string AuthName { get; set; }

        [Required]
        public int AuthValue { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}