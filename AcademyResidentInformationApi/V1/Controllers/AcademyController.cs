using AcademyResidentInformationApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AcademyResidentInformationApi.V1.Boundary.Requests;
using AcademyResidentInformationApi.V1.Domain;

namespace AcademyResidentInformationApi.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/claimants")]
    [Produces("application/json")]
    public class AcademyController : BaseController
    {
        private IGetAllClaimantsUseCase _getAllClaimantsUseCase;
        private readonly IGetClaimantByIdUseCase _getClaimantByIdUseCase;

        public AcademyController(IGetAllClaimantsUseCase getAllClaimantsUseCase, IGetClaimantByIdUseCase getClaimantByIdUseCase)
        {
            _getAllClaimantsUseCase = getAllClaimantsUseCase;
            _getClaimantByIdUseCase = getClaimantByIdUseCase;
        }

        [HttpGet]
        public IActionResult ListContacts([FromQuery] ClaimantQueryParam cqp, int? cursor = 0, int? limit = 20)
        {
            try
            {
                return Ok(_getAllClaimantsUseCase.Execute(cqp, (int) cursor, (int) limit));
            }
            catch (InvalidQueryParameterException e)
            {
                //return 400
                return BadRequest(e.Message);
            }

        }

        [HttpGet]
        [Route("{academyId}")]
        public IActionResult ViewRecord(string academyId)
        {
            return Ok(_getClaimantByIdUseCase.Execute(academyId));
        }


    }
}
