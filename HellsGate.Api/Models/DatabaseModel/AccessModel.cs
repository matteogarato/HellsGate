using System;

namespace HellsGate.Models.DatabaseModel
{
    public class AccessModel : BaseModel
    {
        public int Id { get; set; }
        public DateTime AccessTime { get; set; }
        public bool GrantedAccess { get; set; }
        public string Plate { get; set; }
        public string PeopleEntered { get; set; }
        public string CardNumber { get; set; }
    }
}