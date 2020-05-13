using System.Collections.Generic;

namespace Brasserie.Service.Brasserie.Response
{
    public class GetBrasseriesResponse
    {
        public IEnumerable<Brasserie> Brasseries { get; set; }

        public class Brasserie
        {
            public int Id { get; set; }
            public string Nom { get; set; }
            public IEnumerable<Biere> Bieres { get; set; }
        }

        public class Biere
        {
            public int Id { get; set; }
            public string Nom { get; set; }
            public double DegreAlcool { get; set; }
            public double Prix { get; set; }
            public IEnumerable<Grossiste> Grossistes { get; set; }
        }

        public class Grossiste
        {
            public string Nom { get; set; }
            public int Quantite { get; set; }
        }
    }
}