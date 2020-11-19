using HellsGate.Models.DatabaseModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace HellsGate.Models.Context
{
    public class HellsGateContext : DbContext
    {
        public HellsGateContext()
        {
        }

        public HellsGateContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AccessModel> Access { get; set; }
        public DbSet<AutorizationLevelModel> Autorizations { get; set; }
        public DbSet<CardModel> CardModels { get; set; }
        public DbSet<CarAnagraphicModel> Cars { get; set; }
        public DbSet<IdentityUserClaim<Guid>> IdentityUserClaims { get; set; }
        public DbSet<MainMenuModel> MainMenu { get; set; }
        public DbSet<NodeModel> Nodes { get; set; }
        public DbSet<PersonModel> Peoples { get; set; }
        public DbSet<SafeAuthModel> SafeAuthModels { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<CarAnagraphicModel>(entity =>
            //{
            //    entity.Property(x => x.LicencePlate).HasColumnName("LicencePlate");
            //
            //    entity.OwnsOne(p => p.Owner, cb =>
            //    {
            //        cb.Property(x => x.Id).HasColumnName("Id");
            //    });
            //});
            builder.Entity<PersonModel>(entity =>
            {
                entity.OwnsOne(a => a.SafeAuthModel, sa =>
                   {
                       sa.Property(x => x.Id).HasColumnName("Id");
                   });
                entity.OwnsOne(a => a.AutorizationLevel, al =>
                   {
                       al.Property(x => x.Id).HasColumnName("Id");
                   });
            });

            builder.Entity<IdentityUserClaim<string>>().HasKey(p => new { p.Id });
        }
    }
}