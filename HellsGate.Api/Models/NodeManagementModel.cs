﻿using HellsGate.Models.DatabaseModel;
using System.Collections.Generic;

namespace HellsGate.Models
{
    public class NodeManagementModel
    {
        public WellknownAuthorizationLevel AuthValue { get; set; }
        public string MacAddress { get; set; }
        public string Name { get; set; }
        public List<NodeModel> NodeList { get; set; }
    }
}