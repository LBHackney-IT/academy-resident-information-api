using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Gateways;
using AcademyResidentInformationApi.V1.UseCase.Interfaces;
using MosaicResidentInformationApi.V1.Boundary.Requests;

namespace AcademyResidentInformationApi.V1.UseCase
{
    public class GetAllResidentsUseCase : IGetAllResidentsUseCase
    {
        private readonly IAcademyGateway _academyGateway;

        public GetAllResidentsUseCase(IAcademyGateway academyGateway)
        {
            _academyGateway = academyGateway;
        }

        public ResidentInformationList Execute(ResidentQueryParam rqp, int cursor, int limit)
        {
            limit = limit < 10 ? 10 : limit;
            limit = limit > 100 ? 100 : limit;

            var residents = _academyGateway.GetAllResidents(cursor, limit, rqp.FirstName, rqp.LastName, rqp.Postcode, rqp.Address);
            return new ResidentInformationList
            {
                Residents = residents.ToResponse()
            };
        }
    }
}
