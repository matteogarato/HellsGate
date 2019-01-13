using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HellsGate.Models
{
    public class Context : DbContext
    {
        public DbSet<AccessModel> Access { get; set; }
        public DbSet<CarAnagraphicModel> Cars { get; set; }
        public DbSet<PeopleAnagraphicModel> Peoples { get; set; }
        public DbSet<AutorizationLevelModel> Autorizations { get; set; }
        public DbSet<CardAnagraphicsModel> Cards { get; set; }
    }
}
