using Microsoft.EntityFrameworkCore;
using System;

namespace HellsGate.Models
{
    public enum AuthType
    {
        OneTimeAccess,
        Guest,
        User,
        Admin,
        Root
    }

    /// <summary>
    /// Autorization model for access
    /// </summary>
    [Owned]
    public class AutorizationLevelModel
    {
        public int Id { get; set; }
        public string AuthName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public AuthType AuthValue { get; set; }
    }
}