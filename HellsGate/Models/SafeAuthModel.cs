using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HellsGate.Models
{
    [Owned]
    public class SafeAuthModel
    {

        public int Id { get; set; }
        public byte[] UserSafe { get; set; }
        public byte[] AutSafe { get; set; }
        [NotMapped]
        public PeopleAnagraphicModel User { get; set; }
        [NotMapped]
        public AutorizationLevelModel Auth { get; set; }
        public DateTime DtIns { get; set; }
    }
}
