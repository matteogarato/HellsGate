using System;
using System.Collections.Generic;

namespace HellsGate.Models
{
    public class PeopleAnagraphicModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public virtual CardModel CardNumber { get; set; }
        public DateTime LastModify { get; set; }
        public ICollection<CarAnagraphicModel> Cars { get; set; }
        public virtual AutorizationLevelModel AutorizationLevel { get; set; }
        public SafeAuthModel SafeAuthModel { get; set; }

    }
}
