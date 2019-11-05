using System;
using Microsoft.EntityFrameworkCore;

namespace HellsGate.Models
{
    [Owned]
    public class SafeAuthModel
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public int AutId { get; set; }
        public string Control { get; set; }
        public DateTime DtIns { get; set; }
    }
}
