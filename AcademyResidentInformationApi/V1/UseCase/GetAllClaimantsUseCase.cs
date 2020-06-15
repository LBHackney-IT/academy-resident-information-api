using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Gateways;
using AcademyResidentInformationApi.V1.UseCase.Interfaces;

namespace AcademyResidentInformationApi.V1.UseCase
{
    public class GetAllClaimantsUseCase : IGetAllClaimantsUseCase
    {
        private readonly IAcademyGateway _academyGateway;

        public GetAllClaimantsUseCase(IAcademyGateway academyGateway)
        {
            _academyGateway = academyGateway;
        }

        public ClaimantInformationList Execute()
        {
            var claimants = _academyGateway.GetAllClaimants();
            return new ClaimantInformationList
            {
                Claimants = claimants.ToResponse()
            };
        }
    }
}
