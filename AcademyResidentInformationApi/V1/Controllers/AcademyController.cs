using Microsoft.AspNetCore.Mvc;

namespace AcademyResidentInformationApi.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/residents")]
    [Produces("application/json")]
    public class AcademyController : BaseController
    {
        [HttpGet]
        public IActionResult ListContacts()
        {
            return Ok();
        }

        [HttpGet]
        [Route("{academyId}")]
        public IActionResult ViewRecord(int mosaicId)
        {
            return Ok();
        }
    }
}
