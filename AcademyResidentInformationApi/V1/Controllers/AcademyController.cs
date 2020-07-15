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
        public IActionResult ListContacts([FromQuery] ClaimantQueryParam cqp, string cursor = null, int? limit = 20)
        {
            try
            {
                return Ok(_getAllClaimantsUseCase.Execute(cqp, cursor, (int) limit));
            }
            catch (InvalidQueryParameterException e)
            {
                //return 400
                return BadRequest(e.Message);
            }
            catch (InvalidCursorException e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpGet]
        [Route("claim/{claimId}/person/{personRef}")]
        public IActionResult ViewRecord(int claimId, int personRef)
        {
            try
            {
                var record = _getClaimantByIdUseCase.Execute(claimId, personRef);
                return Ok(record);
            }
            catch (ClaimantNotFoundException)
            {
                //return a 404
                return NotFound();
            }

        }


    }
}
