using System;

namespace HellsGate.Models
{
    public class AccessModel
    {
        public int Id { get; set; }
        public DateTime AccessTime { get; set; }
        public bool GrantedAccess { get; set; }
        public string Plate { get; set; }
        public int PeopleEntered { get; set; }
        public string CardNumber { get; set; }
    }
}
