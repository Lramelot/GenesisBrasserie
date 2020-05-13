using System.Collections.Generic;
using Brasserie.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Brasserie.Data.Contexts
{
    public class BrasserieContext : DbContext
    {
        public DbSet<Core.Domain.Brasserie> Brasseries { get; set; }
        public DbSet<Biere> Bieres { get; set; }
        public DbSet<StockGrossiste> StockGrossistes { get; set; }
        public DbSet<Grossiste> Grossistes { get; set; }

        public BrasserieContext(DbContextOptions<BrasserieContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Biere>()
                .ToTable("Biere");

            modelBuilder
                .Entity<StockGrossiste>()
                .ToTable("StockGrossiste");

            modelBuilder
                .Entity<StockGrossiste>()
                .ToTable("Grossiste");

            modelBuilder
                .Entity<StockGrossiste>()
                .HasOne(sg => sg.Biere)
                .WithMany(b => b.StockGrossistes);

            modelBuilder
                .Entity<StockGrossiste>()
                .HasOne(sg => sg.Grossiste)
                .WithMany(b => b.StockGrossistes);


            modelBuilder
                .Entity<Grossiste>()
                .HasData(new List<Grossiste>
                {
                    new Grossiste
                    {
                        Id = 1,
                        Nom = "GeneDrinks"
                    },

                    new Grossiste
                    {
                        Id = 2,
                        Nom = "Onidrinks"
                    },
                });

            modelBuilder
                .Entity<Core.Domain.Brasserie>()
                .HasData(new List<Core.Domain.Brasserie>
                {
                    new Core.Domain.Brasserie
                    {
                        Id = 1,
                        Nom = "Abbaye de Leffe"
                    },
                    new Core.Domain.Brasserie
                    {
                        Id = 2,
                        Nom = "Abbaye de Chimay"
                    }
                });

            modelBuilder
                .Entity<Biere>()
                .HasData(new Biere
                    {
                        BrasserieId = 1,
                        Id = 1,
                        Nom = "Leffe Blonde",
                        DegreAlcool = 6.6,
                        Prix = 2.2,
                    },
                    new Biere
                    {
                        BrasserieId = 1,
                        Id = 2,
                        Nom = "Leffe Brune",
                        DegreAlcool = 7.6,
                        Prix = 2.8,
                    },
                    new Biere
                    {
                        BrasserieId = 2,
                        Id = 3,
                        Nom = "Chimay Bleue",
                        DegreAlcool = 6.6,
                        Prix = 3.4,
                    },
                    new Biere
                    {
                        BrasserieId = 2,
                        Id = 4,
                        Nom = "Chimay Blonde",
                        DegreAlcool = 6.6,
                        Prix = 3.2,
                    },
                    new Biere
                    {
                        BrasserieId = 2,
                        Id = 5,
                        Nom = "Chimay Rouge",
                        DegreAlcool = 8,
                        Prix = 3,
                    });

            modelBuilder
                .Entity<StockGrossiste>()
                .HasData(new List<StockGrossiste>
                {
                    new StockGrossiste
                    {
                        Id = 1,
                        GrossisteId = 1,
                        BiereId = 1,
                        Quantite = 10
                    },
                    new StockGrossiste
                    {
                        Id = 2,
                        GrossisteId = 1,
                        BiereId = 2,
                        Quantite = 15
                    },
                    new StockGrossiste
                    {
                        Id = 3,
                        GrossisteId = 1,
                        BiereId = 3,
                        Quantite = 10
                    },
                    new StockGrossiste
                    {
                        Id = 4,
                        GrossisteId = 1,
                        BiereId = 4,
                        Quantite = 5
                    },
                    new StockGrossiste
                    {
                        Id = 5,
                        GrossisteId = 1,
                        BiereId = 5,
                        Quantite = 30
                    },
                    new StockGrossiste
                    {
                        Id = 6,
                        GrossisteId = 2,
                        BiereId = 1,
                        Quantite = 10
                    },
                    new StockGrossiste
                    {
                        Id = 7,
                        GrossisteId = 2,
                        BiereId = 3,
                        Quantite = 10
                    },
                    new StockGrossiste
                    {
                        Id = 8,
                        GrossisteId = 1,
                        BiereId = 4,
                        Quantite = 10
                    }
                });
        }
    }
}