using System;
using System.ComponentModel.DataAnnotations;

namespace HellsGate.Models.DatabaseModel
{
    public class NodeModel : BaseModel
    {
        public WellknownAuthorizationLevel AuthValue { get; set; }

        [Key]
        public Guid Id { get; set; }

        public string MacAddress { get; set; }

        public string Name { get; set; }
    }
}