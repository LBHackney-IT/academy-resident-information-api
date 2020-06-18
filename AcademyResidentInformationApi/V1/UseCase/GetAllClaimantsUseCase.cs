using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Gateways;
using AcademyResidentInformationApi.V1.UseCase.Interfaces;
using AcademyResidentInformationApi.V1.Boundary.Requests;

namespace AcademyResidentInformationApi.V1.UseCase
{
    public class GetAllClaimantsUseCase : IGetAllClaimantsUseCase
    {
        private readonly IAcademyGateway _academyGateway;

        public GetAllClaimantsUseCase(IAcademyGateway academyGateway)
        {
            _academyGateway = academyGateway;
        }

        public ClaimantInformationList Execute(ClaimantQueryParam cqp, int cursor, int limit)
        {
            limit = limit < 10 ? 10 : limit;
            limit = limit > 100 ? 100 : limit;

            var claimants = _academyGateway.GetAllClaimants(cursor, limit, cqp.FirstName, cqp.LastName, cqp.Postcode, cqp.Address);
            return new ClaimantInformationList
            {
                Claimants = claimants.ToResponse()
            };
        }
    }
}
