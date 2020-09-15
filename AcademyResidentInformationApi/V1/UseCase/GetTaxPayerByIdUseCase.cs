using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Domain;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Gateways;

namespace AcademyResidentInformationApi.V1.UseCase
{
    public class GetTaxPayerByIdUseCase : IGetTaxPayerByIdUseCase
    {
        private readonly IAcademyGateway _academyGateway;
        public GetTaxPayerByIdUseCase(IAcademyGateway academyGateway)
        {
            _academyGateway = academyGateway;
        }

        public TaxPayerInformationResponse Execute(int accountRef)
        {
            var payerInfo = _academyGateway.GetTaxPayerById(accountRef);
            if (payerInfo == null) throw new TaxPayerNotFoundException();
            return payerInfo.ToResponse();
        }
    }
}
