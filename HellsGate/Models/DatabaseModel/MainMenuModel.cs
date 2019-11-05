using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HellsGate.Models
{
    public class MainMenuModel
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public AuthType AuthLevel { get; set; }
    }
}
