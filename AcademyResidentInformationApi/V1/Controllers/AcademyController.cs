using AcademyResidentInformationApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AcademyResidentInformationApi.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/claimants")]
    [Produces("application/json")]
    public class AcademyController : BaseController
    {
        private IGetAllClaimantsUseCase _getAllClaimantsUseCase;
        public AcademyController(IGetAllClaimantsUseCase getAllClaimantsUseCase)
        {
            _getAllClaimantsUseCase = getAllClaimantsUseCase;

        }
        [HttpGet]
        public IActionResult ListContacts()
        {
            return Ok(_getAllClaimantsUseCase.Execute());

        }

        [HttpGet]
        [Route("{academyId}")]
        public IActionResult ViewRecord()
        {
            return Ok();
        }


    }
}
