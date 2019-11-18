using HellsGate.Lib;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace HellsGate.Models
{
    public class HellsGateContext : DbContext
    {
        private readonly string connectionString;

        public HellsGateContext(string connectionString) : base()
        {
            this.connectionString = connectionString;
        }

        public HellsGateContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer("Server=X201\\SQLEXPRESS;Database=HellsGateDB;Trusted_Connection=True;ConnectRetryCount=0");
            base.OnConfiguring(builder);
        }

        public DbSet<AccessModel> Access { get; set; }
        public DbSet<AutorizationLevelModel> Autorizations { get; set; }
        public DbSet<SafeAuthModel> SafeAuthModels { get; set; }
        public DbSet<CarAnagraphicModel> Cars { get; set; }
        public DbSet<PeopleAnagraphicModel> Peoples { get; set; }
        public DbSet<MainMenuModel> MainMenu { get; set; }
        public DbSet<CardModel> CardModels { get; set; }

        protected override async void OnModelCreating(ModelBuilder builder)
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
            builder.Entity<PeopleAnagraphicModel>(entity =>
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
            builder.Entity<PeopleAnagraphicModel>().HasData(new PeopleAnagraphicModel()
            {
                UserName = "admin",
                Password = await SecurLib.EncryptLineToStringAsync("admin"),
                Id = Guid.NewGuid().ToString()
            });

            //builder.Entity<AutorizationLevelModel>().HasData(new AutorizationLevelModel()
            //{
            //    Id = 1,
            //    AuthName = "ROOT",
            //    AuthValue = AuthType.Root
            //});

            //builder.Entity<SafeAuthModel>().HasData(new SafeAuthModel()
            //{
            //    Id = 1,
            //    AutId = 1,
            //    UserId = 1,
            //    Control = await SecurLib.EncryptLineToStringAsync("1" + "1" + AuthType.Root.ToString()).ConfigureAwait(false)
            //});
        }
    }
}