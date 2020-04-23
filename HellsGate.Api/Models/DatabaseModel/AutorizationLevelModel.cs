using Microsoft.EntityFrameworkCore;
using System;

namespace HellsGate.Models.DatabaseModel
{
    /// <summary>
    /// Autorization model for access
    /// </summary>
    [Owned]
    public class AutorizationLevelModel : BaseModel
    {
        public int Id { get; set; }
        public string AuthName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public WellknownAuthorizationLevel AuthValue { get; set; }
    }
}