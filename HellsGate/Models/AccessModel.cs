using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HellsGate.Models
{
    public class AccessModel
    {
        public int AccessModelId { get; set; }
        public DateTime AccessTime { get; set; }
        public bool GrantedAccess { get; set; }
        public string Plate { get; set; }
        public CarAnagraphicModel CarEntered { get; set; }
    }
}
