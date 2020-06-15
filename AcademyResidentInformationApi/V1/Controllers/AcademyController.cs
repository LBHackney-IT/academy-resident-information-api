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
        private readonly IGetClaimantByIdUseCase _getClaimantByIdUseCase;

        public AcademyController(IGetAllClaimantsUseCase getAllClaimantsUseCase, IGetClaimantByIdUseCase getClaimantByIdUseCase)
        {
            _getAllClaimantsUseCase = getAllClaimantsUseCase;
            _getClaimantByIdUseCase = getClaimantByIdUseCase;
        }

        [HttpGet]
        public IActionResult ListContacts()
        {
            return Ok(_getAllClaimantsUseCase.Execute());

        }

        [HttpGet]
        [Route("{academyId}")]
        public IActionResult ViewRecord(string academyId)
        {
            return Ok(_getClaimantByIdUseCase.Execute(academyId));
        }


    }
}
