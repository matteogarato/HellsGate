using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HellsGate.Models
{
    public class SafeAuthModel
    {

        public int Id { get; set; }
        public byte[] UserSafe { get; set; }
        public byte[] AutSafe { get; set; }
        public PeopleAnagraphicModel User { get; set; }
        public AutorizationLevelModel Auth { get; set; }
        public DateTime DtIns { get; set; }
    }
}
