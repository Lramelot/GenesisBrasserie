using System.Collections;
using System.Collections.Generic;

namespace Brasserie.Core.Domain
{
    public class Biere
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public double DegreAlcool { get; set; }
        public double Prix { get; set; }

        public int BrasserieId { get; set; }
        public virtual Brasserie Brasserie { get; set; }

        public virtual IEnumerable<StockGrossiste> StockGrossistes { get; set; }
    }
}