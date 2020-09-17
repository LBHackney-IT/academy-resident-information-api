using AcademyResidentInformationApi.V1.Boundary.Requests;
using AcademyResidentInformationApi.V1.Domain;
using AcademyResidentInformationApi.V1.UseCase;
using AcademyResidentInformationApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AcademyResidentInformationApi.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/tax-payers")]
    [Produces("application/json")]
    public class TaxPayersController : BaseController
    {
        private readonly IGetTaxPayerByIdUseCase _getTaxPayerByIdUseCase;
        private readonly IGetAllTaxPayersUseCase _getAllTaxPayersUseCase;

        public TaxPayersController(IGetAllTaxPayersUseCase getAllTaxPayersUseCase, IGetTaxPayerByIdUseCase getTaxPayerByIdUseCase)
        {
            _getAllTaxPayersUseCase = getAllTaxPayersUseCase;
            _getTaxPayerByIdUseCase = getTaxPayerByIdUseCase;
        }

        [HttpGet]
        public IActionResult ListTaxPayers([FromQuery] QueryParameters qp)
        {
            var record = _getAllTaxPayersUseCase.Execute(qp);
            return Ok(record);
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
