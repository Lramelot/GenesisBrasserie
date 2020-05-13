using Brasserie.Service.Grossiste;
using Brasserie.Service.Grossiste.Request;
using Microsoft.AspNetCore.Mvc;

namespace Brasserie.Api.Controllers
{
    [Route("api")]
    public class GrossisteController : ControllerBase
    {
        private readonly IGrossisteService _grossisteService;

        public GrossisteController(IGrossisteService grossisteService)
        {
            _grossisteService = grossisteService;
        }

        [HttpPost]
        [Route("brasserie/{brasserieId}/bieres/{biereId}/grossistes")]
        public IActionResult AddBiereToBrasserie([FromRoute] int biereId, [FromBody] AddBiereRequest request)
        {
            request.BiereId = biereId;
            _grossisteService.AddBiere(request);

            return Ok();
        }

        [HttpPost]
        [Route("brasserie/{brasserieId}/bieres/{biereId}/grossistes/{grossisteId}")]
        public IActionResult UpdateStockBiere([FromRoute] int biereId, [FromRoute] int grossisteId, [FromBody] UpdateStockBiereRequest request)
        {
            request.GrossisteId = grossisteId;
            request.BiereId = biereId;

            _grossisteService.UpdateStockBiere(request);

            return Ok();
        }

        [HttpPost]
        [Route("grossiste/{grossisteId}/devis")]
        public IActionResult GetDevis([FromRoute] int grossisteId, [FromBody] GetDevisRequest request)
        {
            request.GrossisteId = grossisteId;
            var devis = _grossisteService.GetDevis(request);

            return Ok(devis);
        }
    }
}