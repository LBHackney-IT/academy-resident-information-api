using AcademyResidentInformationApi.V1.Boundary.Requests;
using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Gateways;
using AcademyResidentInformationApi.V1.UseCase.Interfaces;

namespace AcademyResidentInformationApi.V1.UseCase
{
    public class GetAllTaxPayersUseCase : IGetAllTaxPayersUseCase
    {
        private readonly IAcademyGateway _academyGateway;
        public GetAllTaxPayersUseCase(IAcademyGateway academyGateway)
        {
            _academyGateway = academyGateway;
        }

        public TaxPayerInformationList Execute(QueryParameters qp)
        {
            var taxPayers = _academyGateway.GetAllTaxPayers(qp.FirstName, qp.LastName, qp.Postcode, qp.Address);
            return new TaxPayerInformationList
            {
                TaxPayers = taxPayers.ToResponse()
            };
        }
    }
}
