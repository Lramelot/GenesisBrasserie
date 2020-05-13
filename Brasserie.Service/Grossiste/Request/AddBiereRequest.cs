namespace Brasserie.Service.Grossiste.Request
{
    public class AddBiereRequest
    {
        public int BiereId { get; set; }
        public int GrossisteId { get; set; }
        public int Quantite { get; set; }
    }
}