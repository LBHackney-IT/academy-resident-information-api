using AcademyResidentInformationApi.V1.Boundary.Responses;

namespace AcademyResidentInformationApi.V1.UseCase.Interfaces
{
    public interface IGetResidentByIdUseCase
    {
        ResidentInformation Execute(int id);
    }
}
