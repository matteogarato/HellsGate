using System.ComponentModel.DataAnnotations;

namespace HellsGate.Models.DatabaseModel
{
    public class CarAnagraphicModel : BaseModel
    {
        public string Colour { get; set; }

        [Key]
        public string LicencePlate { get; set; }

        public string Model { get; set; }
        public virtual PersonModel Owner { get; set; }
    }
}