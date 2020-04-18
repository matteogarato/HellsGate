using System;

namespace HellsGate.Models.DatabaseModel
{
    public class BaseModel
    {
        public DateTime? LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastCreated { get; set; }
        public string LastCreatedBy { get; set; }
    }
}