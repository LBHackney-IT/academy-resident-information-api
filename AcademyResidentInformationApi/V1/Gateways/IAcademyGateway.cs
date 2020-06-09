using System.Collections.Generic;
using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Domain;
using ResidentInformation = AcademyResidentInformationApi.V1.Domain.ResidentInformation;

namespace AcademyResidentInformationApi.V1.Gateways
{
    public interface IAcademyGateway
    {
        List<ResidentInformation> GetAllResidents(string postcode = null, string address = null);
        ResidentInformation GetResidentById(int claimId, int personRef);
    }

}
