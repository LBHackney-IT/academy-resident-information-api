using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Gateways;
using AcademyResidentInformationApi.V1.UseCase.Interfaces;

namespace AcademyResidentInformationApi.V1.UseCase
{
    public class GetResidentByIdUseCase : IGetResidentByIdUseCase
    {
        private readonly IAcademyGateway _academyGateway;
        public GetResidentByIdUseCase(IAcademyGateway academyGateway)
        {
            _academyGateway = academyGateway;
        }
        public ResidentInformation Execute(int id)
        {
            return _academyGateway.GetResidentById(id).ToResponse();
        }
    }
}
