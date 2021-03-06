
using AcademyResidentInformationApi.V1.Boundary.Requests;
using AcademyResidentInformationApi.V1.Boundary.Responses;

namespace AcademyResidentInformationApi.V1.UseCase.Interfaces
{
    public interface IGetAllClaimantsUseCase
    {
        ClaimantInformationList Execute(QueryParameters qp, string cursor, int limit);
    }

}
