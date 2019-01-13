using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HellsGate.Models
{
    public class AccessModel
    {
        public int Id { get; set; }
        public DateTime AccessTime { get; set; }
        public bool GrantedAccess { get; set; }
        public virtual CarAnagraphicModel CarEntered { get; set; }
        public virtual PeopleAnagraphicModel PeopleEntered { get; set; }
    }
}
