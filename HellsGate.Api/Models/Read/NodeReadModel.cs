﻿using System;

namespace HellsGate.Models.DatabaseModel
{
    public class NodeReadModel : BaseModel
    {
        public WellknownAuthorizationLevel AuthValue { get; set; }
        public Guid Id { get; set; }

        public string MacAddress { get; set; }

        public string Name { get; set; }

        public string Token { get; set; }
    }
}