using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HellsGate.Api.Models.Read
{
    public class AuthenticateModel
    {
        [Required]
        public int AuthValue { get; set; }

        [Required]
        public string MacAddress { get; set; }

        [Required]
        public string NodeName { get; set; }
    }
}