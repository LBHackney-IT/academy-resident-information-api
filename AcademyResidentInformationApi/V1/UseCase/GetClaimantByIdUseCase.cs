using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Gateways;
using AcademyResidentInformationApi.V1.UseCase.Interfaces;
using ClaimantNotFoundException = AcademyResidentInformationApi.V1.Domain.ClaimantNotFoundException;

namespace AcademyResidentInformationApi.V1.UseCase
{
    public class GetClaimantByIdUseCase : IGetClaimantByIdUseCase
    {
        private readonly IAcademyGateway _academyGateway;
        public GetClaimantByIdUseCase(IAcademyGateway academyGateway)
        {
            _academyGateway = academyGateway;
        }
        public ClaimantInformation Execute(int claimId, int personRef)
        {
            var claimant = _academyGateway.GetClaimantById(claimId, personRef);
            if (claimant == null) throw new ClaimantNotFoundException();
            return claimant.ToResponse();
        }
    }
}
