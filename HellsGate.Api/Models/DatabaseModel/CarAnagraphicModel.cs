using System.ComponentModel.DataAnnotations;

namespace HellsGate.Models.DatabaseModel
{
    public class CarAnagraphicModel : BaseModel
    {
        [Key]
        public string LicencePlate { get; set; }

        public string Model { get; set; }
        public string Colour { get; set; }
        public virtual PeopleAnagraphicModel Owner { get; set; }
    }
}