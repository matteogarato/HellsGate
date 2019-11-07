using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace HellsGate.Models
{
    public class PeopleAnagraphicModel : IdentityUser<string>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public virtual CardModel CardNumber { get; set; }
        public DateTime LastModify { get; set; }
        public virtual ICollection<CarAnagraphicModel> Cars { get; set; }
        public virtual AutorizationLevelModel AutorizationLevel { get; set; }
        public SafeAuthModel SafeAuthModel { get; set; }

        public PeopleAnagraphicModel()
        {
            Id = -1;
        }

    }
}
