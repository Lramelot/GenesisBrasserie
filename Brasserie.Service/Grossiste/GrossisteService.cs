using System.Linq;
using Brasserie.Core.Constant;
using Brasserie.Core.Domain;
using Brasserie.Core.Exception;
using Brasserie.Data.Contexts;
using Brasserie.Service.Grossiste.Request;
using Brasserie.Service.Grossiste.Response;
using Microsoft.EntityFrameworkCore;

namespace Brasserie.Service.Grossiste
{
    public class GrossisteService : IGrossisteService
    {
        private readonly BrasserieContext _brasserieContext;

        public GrossisteService(BrasserieContext brasserieContext)
        {
            _brasserieContext = brasserieContext;
        }

        public void AddBiere(AddBiereRequest request)
        {
            var stockGrossiste = new StockGrossiste
            {
                BiereId = request.BiereId,
                GrossisteId = request.GrossisteId,
                Quantite = request.Quantite
            };

            _brasserieContext.StockGrossistes.Add(stockGrossiste);
            _brasserieContext.SaveChanges();
        }

        public void UpdateStockBiere(UpdateStockBiereRequest request)
        {
            var stockGrossiste = _brasserieContext
                .StockGrossistes
                .Where(sg => sg.BiereId == request.BiereId)
                .Single(sg => sg.GrossisteId == request.GrossisteId);

            stockGrossiste.Quantite = request.Quantite;
            _brasserieContext.SaveChanges();
        }

        public GetDevisResponse GetDevis(GetDevisRequest request)
        {
            var response = new GetDevisResponse();

            if (!request.Commandes.Any())
            {
                throw new ValidationException(DevisValidationMessage.CommandeVide);
            }

            var bieresCommandes = request.Commandes.Select(c => c.BiereId).ToArray();
            if (bieresCommandes.Distinct().Count() != bieresCommandes.Length)
            {
                throw new ValidationException(DevisValidationMessage.BiereDoublon);
            }

            var grossiste = _brasserieContext
                .Grossistes
                .Include(g => g.StockGrossistes)
                .ThenInclude(sg => sg.Biere)
                .FirstOrDefault(g => g.Id == request.GrossisteId);

            if (grossiste == null)
            {
                throw new ValidationException(DevisValidationMessage.GrossisteInexistant);
            }

            var bieresGrossiste = grossiste.StockGrossistes.Select(sg => sg.BiereId).Distinct();

            if (!bieresCommandes.All(b => bieresGrossiste.Contains(b)))
            {
                throw new ValidationException(DevisValidationMessage.StockIncomplet);
            }

            foreach (var commande in request.Commandes)
            {
                var biereGrossiste = grossiste.StockGrossistes.First(sg => sg.BiereId == commande.BiereId);

                if (biereGrossiste.Quantite < commande.Quantite)
                {
                    throw new ValidationException(DevisValidationMessage.StockInsuffisant);
                }

                response.LignesDevis.Add(new GetDevisResponse.LigneDevis
                {
                    Biere = biereGrossiste.Biere.Nom,
                    Quantite = commande.Quantite,
                    PrixUnitaire = biereGrossiste.Biere.Prix
                });
            }

            if (response.QuantiteTotale > 10)
            {
                response.Reduction = 10;
            }
            if (response.QuantiteTotale > 20)
            {
                response.Reduction = 20;
            }

            return response;
        }
    }
}