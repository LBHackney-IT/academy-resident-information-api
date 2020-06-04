using AcademyResidentInformationApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult ListContacts()
        {
            return Ok(_getAllResidentsUseCase.Execute());

        }

        [HttpGet]
        [Route("{academyId}")]
        public IActionResult ViewRecord()
        {
            return Ok();
        }


    }
}
