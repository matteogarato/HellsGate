using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace HellsGate.Models.DatabaseModel
{
    [Owned]
    public class SafeAuthModel : BaseModel
    {
        public Guid AutId { get; set; }

        public string Control { get; set; }

        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
    }
}