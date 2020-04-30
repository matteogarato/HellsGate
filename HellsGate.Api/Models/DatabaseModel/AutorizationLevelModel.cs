using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace HellsGate.Models.DatabaseModel
{
    /// <summary>
    /// Autorization model for access
    /// </summary>
    [Owned]
    public class AutorizationLevelModel : BaseModel
    {
        public string AuthName { get; set; }

        public WellknownAuthorizationLevel AuthValue { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        [Key]
        public Guid Id { get; set; }
    }
}