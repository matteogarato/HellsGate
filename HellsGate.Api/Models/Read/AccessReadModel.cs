using System;
using System.Collections.Generic;
using System.Text;

namespace HellsGate.Api.Models.Read
{
    public class AccessReadModel
    {
        public string CardNumber { get; set; }
        public string MacAddress { get; set; }
        public string NodeName { get; set; }
    }
}