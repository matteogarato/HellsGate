using System;

namespace HellsGate.Models.DatabaseModel
{
    public class NodeUpdateModel : BaseModel
    {
        public Guid Id { get; set; }

        public string MacAddress { get; set; }

        public string Name { get; set; }

        public WellknownAuthorizationLevel AuthValue { get; set; }
    }
}