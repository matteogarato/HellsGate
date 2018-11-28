using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HellsGate.Models
{
    public class PeopleAnagraphicModel
    {

        public int PeopleAnagraphicModelId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime LastModify { get; set; }
        public AutorizationLevelModel Auth { get; set; }
    }
}
