using Brasserie.Service.Brasserie.Request;
using Brasserie.Service.Brasserie.Response;

namespace Brasserie.Service.Brasserie
{
    public interface IBrasserieService
    {
        GetBrasseriesResponse GetBrasseries();
        void CreateBiere(CreateBiereRequest request);
        void DeleteBiere(int biereId);
    }
}