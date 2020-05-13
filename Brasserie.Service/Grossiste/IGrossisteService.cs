using Brasserie.Service.Grossiste.Request;
using Brasserie.Service.Grossiste.Response;

namespace Brasserie.Service.Grossiste
{
    public interface IGrossisteService
    {
        void AddBiere(AddBiereRequest request);
        void UpdateStockBiere(UpdateStockBiereRequest request);
        GetDevisResponse GetDevis(GetDevisRequest request);
    }
}