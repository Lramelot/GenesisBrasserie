using System.Collections.Generic;

namespace Brasserie.Core.Domain
{
    public class Brasserie
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public ICollection<Biere> Bieres { get; set; }
    }
}