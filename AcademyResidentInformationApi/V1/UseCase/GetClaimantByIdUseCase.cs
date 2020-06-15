using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Gateways;
using AcademyResidentInformationApi.V1.UseCase.Interfaces;

namespace AcademyResidentInformationApi.V1.UseCase
{
    public class GetClaimantByIdUseCase : IGetClaimantByIdUseCase
    {
        private readonly IAcademyGateway _academyGateway;
        public GetClaimantByIdUseCase(IAcademyGateway academyGateway)
        {
            _academyGateway = academyGateway;
        }
        public ClaimantInformation Execute(string academyId)
        {
            var compositeKeyArray = academyId.Split('-');
            var claimId = int.Parse(compositeKeyArray[0]);
            var personRef = int.Parse(compositeKeyArray[1]);

            return _academyGateway.GetClaimantById(claimId, personRef).ToResponse();
        }
    }
}
