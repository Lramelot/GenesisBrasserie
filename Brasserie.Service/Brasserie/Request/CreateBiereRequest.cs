namespace Brasserie.Service.Brasserie.Request
{
    public class CreateBiereRequest
    {
        public int BrasserieId { get; set; }
        public string Nom { get; set; }
        public double DegreAlcool { get; set; }
        public double Prix { get; set; }
    }
}