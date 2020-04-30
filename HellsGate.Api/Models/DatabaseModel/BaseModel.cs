using System;

namespace HellsGate.Models.DatabaseModel
{
    public class BaseModel
    {
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}