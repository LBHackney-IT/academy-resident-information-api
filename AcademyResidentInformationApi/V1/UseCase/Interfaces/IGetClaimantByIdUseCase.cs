using AcademyResidentInformationApi.V1.Boundary.Responses;

namespace AcademyResidentInformationApi.V1.UseCase.Interfaces
{
    public interface IGetClaimantByIdUseCase
    {
        ClaimantInformation Execute(int claimId, int personRef);
    }
}
