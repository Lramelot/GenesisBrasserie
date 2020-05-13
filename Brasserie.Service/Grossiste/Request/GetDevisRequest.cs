using System.Collections;
using System.Collections.Generic;

namespace Brasserie.Service.Grossiste.Request
{
    public class GetDevisRequest
    {
        public IEnumerable<Commande> Commandes { get; set; } = new List<Commande>();
        public int GrossisteId { get; set; }

        public class Commande
        {
            public int BiereId { get; set; }
            public int Quantite { get; set; }
        }
    }
}