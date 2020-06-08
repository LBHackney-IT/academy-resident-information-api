using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Gateways;
using AcademyResidentInformationApi.V1.UseCase.Interfaces;

namespace AcademyResidentInformationApi.V1.UseCase
{
    public class GetAllResidentsUseCase : IGetAllResidentsUseCase
    {
        private readonly IAcademyGateway _academyGateway;

        public GetAllResidentsUseCase(IAcademyGateway academyGateway)
        {
            _academyGateway = academyGateway;
        }

        public ResidentInformationList Execute()
        {
            var residents = _academyGateway.GetAllResidents();
            return new ResidentInformationList
            {
                Residents = residents.ToResponse()
            };
        }
    }
}
