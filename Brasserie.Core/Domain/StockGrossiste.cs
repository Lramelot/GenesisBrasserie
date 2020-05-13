namespace Brasserie.Core.Domain
{
    public class StockGrossiste
    {
        public int Id { get; set; }

        public int GrossisteId { get; set; }
        public virtual Grossiste Grossiste { get; set; }

        public int BiereId { get; set; }
        public virtual Biere Biere { get; set; }

        public int Quantite { get; set; }
    }
}