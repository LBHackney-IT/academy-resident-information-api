using AcademyResidentInformationApi.V1.Domain;

namespace AcademyResidentInformationApi.V1.Gateways
{
    public interface IExampleGateway
    {
        Entity GetEntityById(int id);
    }
}
