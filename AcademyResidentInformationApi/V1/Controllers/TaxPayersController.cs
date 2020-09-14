using AcademyResidentInformationApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AcademyResidentInformationApi.V1.Boundary.Requests;
using AcademyResidentInformationApi.V1.Domain;
using AcademyResidentInformationApi.V1.UseCase;

namespace AcademyResidentInformationApi.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/tax-payers")]
    [Produces("application/json")]
    public class TaxPayersController : BaseController
    {
        private readonly IGetTaxPayerByIdUseCase _getTaxPayerByIdUseCase;

        public TaxPayersController(IGetTaxPayerByIdUseCase getTaxPayerByIdUseCase)
        {
            _getTaxPayerByIdUseCase = getTaxPayerByIdUseCase;
        }

        [HttpGet]
        public IActionResult ListTaxPayers()
        {
            return Ok();
        }

        [HttpGet]
        [Route("{accountRef}")]
        public IActionResult ViewRecord(int accountRef)
        {
            try
            {
                var record = _getTaxPayerByIdUseCase.Execute(accountRef);
                return Ok(record);
            }
            catch (TaxPayerNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
