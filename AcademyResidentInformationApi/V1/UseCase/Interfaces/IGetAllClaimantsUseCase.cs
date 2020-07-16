
using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Boundary.Requests;

namespace AcademyResidentInformationApi.V1.UseCase.Interfaces
{
    public interface IGetAllClaimantsUseCase
    {
        ClaimantInformationList Execute(ClaimantQueryParam cqp, string cursor, int limit);
    }

}
