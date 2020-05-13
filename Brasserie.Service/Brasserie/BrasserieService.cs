using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Brasserie.Core.Domain;
using Brasserie.Data.Contexts;
using Brasserie.Service.Brasserie.Request;
using Brasserie.Service.Brasserie.Response;
using Brasserie.Service.Grossiste.Request;
using Microsoft.EntityFrameworkCore;

namespace Brasserie.Service.Brasserie
{
    public class BrasserieService : IBrasserieService
    {
        private readonly BrasserieContext _brasserieContext;
        private readonly IMapper _mapper;

        public BrasserieService(BrasserieContext brasserieContext, IMapper mapper)
        {
            _brasserieContext = brasserieContext;
            _mapper = mapper;
        }

        public GetBrasseriesResponse GetBrasseries()
        {
            var brasseries = _brasserieContext
                .Brasseries
                .Include(b => b.Bieres)
                .ThenInclude(b => b.StockGrossistes)
                .ThenInclude(sg => sg.Grossiste);

            var response = new GetBrasseriesResponse
            {
                Brasseries = _mapper.Map<IEnumerable<GetBrasseriesResponse.Brasserie>>(brasseries)
            };

            return response;
        }

        public void CreateBiere(CreateBiereRequest request)
        {
            _brasserieContext.Bieres.Add(new Biere
            {
                DegreAlcool = request.DegreAlcool,
                Nom = request.Nom,
                Prix = request.Prix,
                BrasserieId = request.BrasserieId
            });

            _brasserieContext.SaveChanges();
        }

        public void DeleteBiere(int biereId)
        {
            var biere = _brasserieContext
                .Bieres
                .First(b => b.Id == biereId);

            _brasserieContext.Bieres.Remove(biere);
            _brasserieContext.SaveChanges();
        }
    }
}