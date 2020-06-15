using System.Collections.Generic;
using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Domain;
using ClaimantInformation = AcademyResidentInformationApi.V1.Domain.ClaimantInformation;

namespace AcademyResidentInformationApi.V1.Gateways
{
    public interface IAcademyGateway
    {
        List<ClaimantInformation> GetAllClaimants(string postcode = null, string address = null);
    }

}
