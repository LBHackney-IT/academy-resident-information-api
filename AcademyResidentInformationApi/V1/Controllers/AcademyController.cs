using AcademyResidentInformationApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MosaicResidentInformationApi.V1.Boundary.Requests;

namespace AcademyResidentInformationApi.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/residents")]
    [Produces("application/json")]
    public class AcademyController : BaseController
    {
        private IGetAllResidentsUseCase _getAllResidentsUseCase;
        public AcademyController(IGetAllResidentsUseCase getAllResidentsUseCase)
        {
            _getAllResidentsUseCase = getAllResidentsUseCase;

        }
        [HttpGet]
        public IActionResult ListContacts([FromQuery] ResidentQueryParam rqp, int? cursor = 0, int? limit = 20)
        {
            return Ok(_getAllResidentsUseCase.Execute(rqp, (int) cursor, (int) limit));

        }

        [HttpGet]
        [Route("{academyId}")]
        public IActionResult ViewRecord()
        {
            return Ok();
        }


    }
}
