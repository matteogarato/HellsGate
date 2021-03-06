﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HellsGate.Models.DatabaseModel
{
    public class PersonModel : IdentityUser<Guid>
    {
        public virtual AutorizationLevelModel AutorizationLevel { get; set; }

        public virtual CardModel CardNumber { get; set; }

        public virtual ICollection<CarAnagraphicModel> Cars { get; set; }

        public override string Email { get; set; }

        [Key]
        public override Guid Id { get; set; }

        public DateTime LastModify { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public virtual SafeAuthModel SafeAuthModel { get; set; }
        public string Surname { get; set; }
        public override string UserName { get; set; }
    }
}