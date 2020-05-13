using Brasserie.Service.Brasserie;
using Brasserie.Service.Brasserie.Request;
using Microsoft.AspNetCore.Mvc;

namespace Brasserie.Api.Controllers
{
    [Route("api/brasserie")]
    public class BrasserieController : ControllerBase
    {
        private readonly IBrasserieService _brasserieService;

        public BrasserieController(IBrasserieService brasserieService)
        {
            _brasserieService = brasserieService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetBrasseries()
        {
            var brasseries = _brasserieService.GetBrasseries();
            return Ok(brasseries);
        }

        [HttpPost]
        [Route("{brasserieId}/bieres")]
        public IActionResult CreateBiere([FromRoute] int brasserieId, [FromBody] CreateBiereRequest request)
        {
            request.BrasserieId = brasserieId;
            _brasserieService.CreateBiere(request);

            return Ok();
        }

        [HttpDelete]
        [Route("{brasserieId}/bieres/{biereId}")]
        public IActionResult DeleteBiere([FromRoute] int biereId)
        {
            _brasserieService.DeleteBiere(biereId);

            return Ok();
        }
    }
}