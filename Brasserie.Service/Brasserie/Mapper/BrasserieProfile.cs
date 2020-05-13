using AutoMapper;
using Brasserie.Core.Domain;
using Brasserie.Service.Brasserie.Response;

namespace Brasserie.Service.Brasserie.Mapper
{
    public class BrasserieProfile : Profile
    {
        public BrasserieProfile()
        {
            CreateMap<Core.Domain.Brasserie, GetBrasseriesResponse.Brasserie>();
            CreateMap<Core.Domain.Biere, GetBrasseriesResponse.Biere>()
                .ForMember(g => g.Grossistes, opt => opt.MapFrom(b => b.StockGrossistes));
            CreateMap<StockGrossiste, GetBrasseriesResponse.Grossiste>()
                .ForMember(g => g.Nom, opt => opt.MapFrom(sg => sg.Grossiste.Nom));
        }
    }
}