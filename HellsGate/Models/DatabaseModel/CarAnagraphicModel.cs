using System;
using System.ComponentModel.DataAnnotations;

namespace HellsGate.Models
{
    public class CarAnagraphicModel
    {
        [Key]
        public string LicencePlate { get; set; }
        public string Model { get; set; }
        public string Colour { get; set; }
        public DateTime LastModify { get; set; }
        public virtual PeopleAnagraphicModel Owner { get; set; }
    }
}
