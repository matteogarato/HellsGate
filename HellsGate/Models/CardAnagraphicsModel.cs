using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HellsGate.Models
{
    public class CardAnagraphicsModel
    {
        [Key]
        public string CardNumber { get; set; }
        public DateTime LastModify { get; set; }
        public virtual PeopleAnagraphicModel people { get; set; }
    }
}
