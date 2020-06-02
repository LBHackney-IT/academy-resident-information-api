using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Gateways;
using AcademyResidentInformationApi.V1.UseCase.Interfaces;

namespace AcademyResidentInformationApi.V1.UseCase
{
    public class GetResidentByIdUseCase : IGetResidentByIdUseCase
    {
        private IAcademyGateway _academyGateway;
        public GetResidentByIdUseCase(IAcademyGateway academyGateway)
        {
            _academyGateway = academyGateway;
        }
        public ResidentInformation Execute(int id)
        {
            //TODO:
            //Return response object using factory extension method
            return _academyGateway.GetResidentById(id); //.ToResponse()
        }
    }
}
