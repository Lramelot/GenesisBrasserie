using System;
using System.Collections.Generic;
using AutoFixture;
using Brasserie.Core.Constant;
using Brasserie.Core.Domain;
using Brasserie.Core.Exception;
using Brasserie.Data.Contexts;
using Brasserie.Service.Grossiste;
using Brasserie.Service.Grossiste.Request;
using Brasserie.Service.Grossiste.Response;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Brasserie.Service.Tests
{
    public class GrossisteServiceTests : TestingContext<GrossisteService>
    {
        public GrossisteServiceTests()
        {
            base.Setup();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public void GetDevis_DoitLancerUneException_QuandCommandeVide()
        {
            // Arrange
            var request = new GetDevisRequest();
            InjectClassFor(InitializeContext());

            // Act
            Action action = () => ClassUnderTest.GetDevis(request);

            // Assert
            action.Should()
                .Throw<ValidationException>()
                .WithMessage(DevisValidationMessage.CommandeVide);
        }

        [Fact]
        public void GetDevis_DoitLancerUneException_QuandDoublonBiereCommandee()
        {
            // Arrange
            var request = new GetDevisRequest
            {
                Commandes = new List<GetDevisRequest.Commande>
                {
                    new GetDevisRequest.Commande { BiereId = 1, Quantite = 10 },
                    new GetDevisRequest.Commande { BiereId = 2, Quantite = 10 },
                    new GetDevisRequest.Commande { BiereId = 1, Quantite = 10 },
                }
            };
            InjectClassFor(InitializeContext());

            // Act
            Action action = () => ClassUnderTest.GetDevis(request);

            // Assert
            action.Should()
                .Throw<ValidationException>()
                .WithMessage(DevisValidationMessage.BiereDoublon);
        }

        [Fact]
        public void GetDevis_DoitLancerUneException_QuandGrossisteExistePas()
        {
            // Arrange
            var request = new GetDevisRequest
            {
                Commandes = fixture.Build<GetDevisRequest.Commande>().CreateMany()
            };
            InjectClassFor(InitializeContext());

            // Act
            Action action = () => ClassUnderTest.GetDevis(request);

            // Assert
            action.Should()
                .Throw<ValidationException>()
                .WithMessage(DevisValidationMessage.GrossisteInexistant);
        }

        [Fact]
        public void GetDevis_DoitLancerUneException_QuandGrossisteStockIncomplet()
        {
            // Arrange
            var grossiste = fixture.Create<Core.Domain.Grossiste>();
            var request = new GetDevisRequest
            {
                GrossisteId = grossiste.Id,
                Commandes = new List<GetDevisRequest.Commande>
                {
                    new GetDevisRequest.Commande { BiereId = 1, Quantite = 10 },
                    new GetDevisRequest.Commande { BiereId = 2, Quantite = 10 },
                    new GetDevisRequest.Commande { BiereId = 3, Quantite = 10 },
                }
            };

            var context = InitializeContext();

            context.Grossistes.Add(grossiste);
            context.StockGrossistes.Add(new StockGrossiste { Id = 1, BiereId = 1, GrossisteId = grossiste.Id });
            context.StockGrossistes.Add(new StockGrossiste { Id = 2, BiereId = 2, GrossisteId = grossiste.Id });
            context.SaveChanges();

            InjectClassFor(context);

            // Act
            Action action = () => ClassUnderTest.GetDevis(request);

            // Assert
            action.Should()
                .Throw<ValidationException>()
                .WithMessage(DevisValidationMessage.StockIncomplet);
        }

        [Fact]
        public void GetDevis_DoitLancerUneException_QuandGrossisteStockInsufissant()
        {
            // Arrange
            var grossiste = fixture.Create<Core.Domain.Grossiste>();
            var request = new GetDevisRequest
            {
                GrossisteId = grossiste.Id,
                Commandes = new List<GetDevisRequest.Commande>
                {
                    new GetDevisRequest.Commande { BiereId = 1, Quantite = 2 },
                    new GetDevisRequest.Commande { BiereId = 2, Quantite = 15 },
                }
            };

            var chimayBleue = new Biere { Id = 1, Prix = 2.8, Nom = "Chimay Bleue" };
            var chimayBleueExtra = new Biere { Id = 2, Prix = 3.8, Nom = "Chimay Bleue Extra" };

            var context = InitializeContext();

            context.Grossistes.Add(grossiste);
            context.Bieres.Add(chimayBleue);
            context.Bieres.Add(chimayBleueExtra);
            context.StockGrossistes.Add(new StockGrossiste { Id = 1, BiereId = 1, Quantite = 5, GrossisteId = grossiste.Id });
            context.StockGrossistes.Add(new StockGrossiste { Id = 2, BiereId = 2, Quantite = 10, GrossisteId = grossiste.Id });
            context.SaveChanges();

            InjectClassFor(context);

            // Act
            Action action = () => ClassUnderTest.GetDevis(request);

            // Assert
            action.Should()
                .Throw<ValidationException>()
                .WithMessage(DevisValidationMessage.StockInsuffisant);
        }

        [Fact]
        public void GetDevis_DoitRenvoyerDevisSansReduction_QuandInferieur10()
        {
            // Arrange
            var grossiste = fixture.Create<Core.Domain.Grossiste>();
            var request = new GetDevisRequest
            {
                GrossisteId = grossiste.Id,
                Commandes = new List<GetDevisRequest.Commande>
                {
                    new GetDevisRequest.Commande { BiereId = 1, Quantite = 2 },
                    new GetDevisRequest.Commande { BiereId = 2, Quantite = 3 },
                }
            };

            var chimayBleue = new Biere {Id = 1, Prix = 2.8, Nom = "Chimay Bleue"};
            var chimayBleueExtra = new Biere {Id = 2, Prix = 3.8, Nom = "Chimay Bleue Extra"}; 

            var context = InitializeContext();

            context.Grossistes.Add(grossiste);
            context.Bieres.Add(chimayBleue);
            context.Bieres.Add(chimayBleueExtra);
            context.StockGrossistes.Add(new StockGrossiste { Id = 1, BiereId = 1, Quantite = 5, GrossisteId = grossiste.Id });
            context.StockGrossistes.Add(new StockGrossiste { Id = 2, BiereId = 2, Quantite = 10, GrossisteId = grossiste.Id });
            context.SaveChanges();

            InjectClassFor(context);

            // Act
            var response = ClassUnderTest.GetDevis(request);

            // Assert
            response.LignesDevis[0].Biere.Should().Be(chimayBleue.Nom);
            response.LignesDevis[0].Quantite.Should().Be(2);
            response.LignesDevis[0].PrixUnitaire.Should().Be(chimayBleue.Prix);

            response.LignesDevis[1].Biere.Should().Be(chimayBleueExtra.Nom);
            response.LignesDevis[1].Quantite.Should().Be(3);
            response.LignesDevis[1].PrixUnitaire.Should().Be(chimayBleueExtra.Prix);

            response.PrixFinal.Should().Be(2 * chimayBleue.Prix + 3 * chimayBleueExtra.Prix);
        }

        [Fact]
        public void GetDevis_DoitRenvoyerDevisAvecReduction_QuandSuperieur10()
        {
            // Arrange
            var grossiste = fixture.Create<Core.Domain.Grossiste>();
            var request = new GetDevisRequest
            {
                GrossisteId = grossiste.Id,
                Commandes = new List<GetDevisRequest.Commande>
                {
                    new GetDevisRequest.Commande { BiereId = 1, Quantite = 5 },
                    new GetDevisRequest.Commande { BiereId = 2, Quantite = 10 },
                }
            };

            var chimayBleue = new Biere { Id = 1, Prix = 2.8, Nom = "Chimay Bleue" };
            var chimayBleueExtra = new Biere { Id = 2, Prix = 3.8, Nom = "Chimay Bleue Extra" };

            var context = InitializeContext();

            context.Grossistes.Add(grossiste);
            context.Bieres.Add(chimayBleue);
            context.Bieres.Add(chimayBleueExtra);
            context.StockGrossistes.Add(new StockGrossiste { Id = 1, BiereId = 1, Quantite = 5, GrossisteId = grossiste.Id });
            context.StockGrossistes.Add(new StockGrossiste { Id = 2, BiereId = 2, Quantite = 10, GrossisteId = grossiste.Id });
            context.SaveChanges();

            InjectClassFor(context);

            // Act
            var response = ClassUnderTest.GetDevis(request);

            // Assert
            response.PrixFinal.Should().Be((5 * chimayBleue.Prix + 10 * chimayBleueExtra.Prix) * 0.9);
        }

        [Fact]
        public void GetDevis_DoitRenvoyerDevisAvecReduction_QuandSuperieur20()
        {
            // Arrange
            var grossiste = fixture.Create<Core.Domain.Grossiste>();
            var request = new GetDevisRequest
            {
                GrossisteId = grossiste.Id,
                Commandes = new List<GetDevisRequest.Commande>
                {
                    new GetDevisRequest.Commande { BiereId = 1, Quantite = 15 },
                    new GetDevisRequest.Commande { BiereId = 2, Quantite = 10 },
                }
            };

            var chimayBleue = new Biere { Id = 1, Prix = 2.8, Nom = "Chimay Bleue" };
            var chimayBleueExtra = new Biere { Id = 2, Prix = 3.8, Nom = "Chimay Bleue Extra" };

            var context = InitializeContext();

            context.Grossistes.Add(grossiste);
            context.Bieres.Add(chimayBleue);
            context.Bieres.Add(chimayBleueExtra);
            context.StockGrossistes.Add(new StockGrossiste { Id = 1, BiereId = 1, Quantite = 15, GrossisteId = grossiste.Id });
            context.StockGrossistes.Add(new StockGrossiste { Id = 2, BiereId = 2, Quantite = 10, GrossisteId = grossiste.Id });
            context.SaveChanges();

            InjectClassFor(context);

            // Act
            var response = ClassUnderTest.GetDevis(request);

            // Assert
            response.PrixFinal.Should().Be((15 * chimayBleue.Prix + 10 * chimayBleueExtra.Prix) * 0.8);
        }

        private BrasserieContext InitializeContext()
        {
            var options = new DbContextOptionsBuilder<BrasserieContext>()
                .UseInMemoryDatabase(databaseName: "BrasserieContextDatabase")
                .Options;

            var context = new BrasserieContext(options);
            context.Database.EnsureDeleted();
            return context;
        }
    }
}