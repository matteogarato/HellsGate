using System.ComponentModel.DataAnnotations;

namespace HellsGate.Models.DatabaseModel
{
    public class NodeModel : BaseModel
    {
        [Key]
        public string Id { get; set; }

        public string MacAddress { get; set; }

        public string Name { get; set; }

        public WellknownAuthorizationLevel AuthValue { get; set; }
    }
}