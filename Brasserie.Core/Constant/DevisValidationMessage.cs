namespace Brasserie.Core.Constant
{
    public class DevisValidationMessage
    {
        public static string CommandeVide = "La commande ne peut pas être vide";
        public static string GrossisteInexistant = "Le grossiste demandé n'existe pas";
        public static string BiereDoublon = "Il y a un doublon dans les bieres commandées";
        public static string StockIncomplet = "Le grossiste ne possède pas toutes les bieres demandées";
        public static string StockInsuffisant = "Le grossiste ne possède pas autant de stock que demandé";
    }
}