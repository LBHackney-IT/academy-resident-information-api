using System.Collections.Generic;
using AcademyResidentInformationApi.V1.Domain;
using AcademyResidentInformationApi.V1.Infrastructure;
using ClaimantInformation = AcademyResidentInformationApi.V1.Domain.ClaimantInformation;

namespace AcademyResidentInformationApi.V1.Gateways
{
    public interface IAcademyGateway
    {
        List<ClaimantInformation> GetAllClaimants(Cursor cursor, int limit, string firstname = null,
            string lastname = null, string postcode = null, string address = null);
        ClaimantInformation GetClaimantById(int claimId, int personRef);
        List<TaxPayerInformation> GetAllTaxPayers(string firstname = null, string lastname = null, string address = null, string postcode = null);
        TaxPayerInformation GetTaxPayerById(int accountRef);
    }

}
