using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HellsGate.Models
{
    public enum AuthType
    {
        Root,
        Admin,
        User,
        Guest,
        OneTimeAccess
    }
    /// <summary>
    /// Autorization model for access
    /// </summary>
    public class AutorizationLevelModel
    {
        public int AutorizationLevelModelId { get; set; }
        public string AuthName { get; set; }
        public AuthType AuthValue { get; set; }
    }
}
