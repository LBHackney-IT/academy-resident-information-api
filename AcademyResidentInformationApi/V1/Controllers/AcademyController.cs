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
        private IGetResidentByIdUseCase _getResidentByIdUseCase;
        public AcademyController(IGetAllResidentsUseCase getAllResidentsUseCase, IGetResidentByIdUseCase getResidentByIdUseCase)
        {
            _getAllResidentsUseCase = getAllResidentsUseCase;
            _getResidentByIdUseCase = getResidentByIdUseCase;
        }

        [HttpGet]
        public IActionResult ListContacts()
        {
            return Ok(_getAllResidentsUseCase.Execute());

        }

        [HttpGet]
        [Route("{academyId}")]
        public IActionResult ViewRecord(int academyId)
        {
            return Ok(_getResidentByIdUseCase.Execute(academyId));
        }


    }
}
