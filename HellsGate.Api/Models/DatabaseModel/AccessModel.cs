using System;

namespace HellsGate.Models.DatabaseModel
{
    public class AccessModel : BaseModel
    {
        public DateTime AccessTime { get; set; }
        public string CardNumber { get; set; }
        public bool GrantedAccess { get; set; }
        public int Id { get; set; }
        public Guid PeopleEntered { get; set; }
        public string Plate { get; set; }
    }
}