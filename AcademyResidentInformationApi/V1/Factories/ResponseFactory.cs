using System.Collections.Generic;
using System.Linq;
using AcademyResidentInformationApi.V1.Domain;
using AddressResponse = AcademyResidentInformationApi.V1.Boundary.Responses.Address;
using ClaimantInformationResponse = AcademyResidentInformationApi.V1.Boundary.Responses.ClaimantInformation;


namespace AcademyResidentInformationApi.V1.Factories
{
    public static class ResponseFactory
    {
        public static ClaimantInformationResponse ToResponse(this ClaimantInformation domain)
        {
            return new ClaimantInformationResponse
            {
                ClaimId = domain.ClaimId,
                CheckDigit = domain.CheckDigit,
                PersonRef = domain.PersonRef,
                FirstName = domain.FirstName,
                LastName = domain.LastName,
                DateOfBirth = domain.DateOfBirth,
                NINumber = domain.NINumber,
                ClaimantAddress = domain.ClaimantAddress.ToResponse(),
            };
        }

        public static List<ClaimantInformationResponse> ToResponse(this IEnumerable<ClaimantInformation> people)
        {
            return people.Select(p => p.ToResponse()).ToList();
        }

        private static AddressResponse ToResponse(this Address claimantAddress)
        {
            return new AddressResponse()
            {
                AddressLine1 = claimantAddress.AddressLine1,
                AddressLine2 = claimantAddress.AddressLine2,
                AddressLine3 = claimantAddress.AddressLine3,
                AddressLine4 = claimantAddress.AddressLine4,
                Postcode = claimantAddress.PostCode
            };
        }
    }
}
