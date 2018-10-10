using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HellsGate.Models
{
    public class CarAnagraphicModel
    {
        public int CarAnagraphicModelId { get; set; }
        public string LicencePlate { get; set; }
        public PeopleAnagraphicModel Owner { get; set; }
        public AutorizationLevelModel AutorizationLevel { get; set; }

    }
}
