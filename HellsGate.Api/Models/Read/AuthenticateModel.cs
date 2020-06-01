using System.ComponentModel.DataAnnotations;

namespace HellsGate.Api.Models.Read
{
    public class AuthenticateModel
    {
        [Required]
        public string MacAddress { get; set; }

        [Required]
        public string NodeName { get; set; }
    }
}