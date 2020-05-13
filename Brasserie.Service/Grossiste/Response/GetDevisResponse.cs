using System.Collections.Generic;
using System.Linq;

namespace Brasserie.Service.Grossiste.Response
{
    public class GetDevisResponse
    {
        public int QuantiteTotale => LignesDevis.Sum(ld => ld.Quantite);
        public double PrixTotal => LignesDevis.Sum(ld => ld.Prix);
        public double Reduction { get; set; } = 0;
        public double PrixFinal => (PrixTotal / 100 * (100 - Reduction));
        public IList<LigneDevis> LignesDevis { get; set; } = new List<LigneDevis>();

        public class LigneDevis
        {
            public string Biere { get; set; }
            public int Quantite { get; set; }
            public double PrixUnitaire { get; set; }
            public double Prix => PrixUnitaire * Quantite;
        }
    }
}