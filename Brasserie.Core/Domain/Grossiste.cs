using System.Collections;
using System.Collections.Generic;

namespace Brasserie.Core.Domain
{
    public class Grossiste
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        public virtual ICollection<StockGrossiste> StockGrossistes { get; set; }
    }
}