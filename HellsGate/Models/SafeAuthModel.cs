﻿using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HellsGate.Models
{
    [Owned]
    public class SafeAuthModel
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public int AutId { get; set; }

        public string Control { get; set; }
        [NotMapped]
        public PeopleAnagraphicModel User { get; set; }
        [NotMapped]
        public AutorizationLevelModel Auth { get; set; }
        public DateTime DtIns { get; set; }
    }
}
