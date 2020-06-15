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
                FirstName = domain.FirstName,
                LastName = domain.LastName,
                DateOfBirth = domain.DateOfBirth,
                AddressList = domain.AddressList.ToResponse(),
            };
        }

        public static List<ClaimantInformationResponse> ToResponse(this IEnumerable<ClaimantInformation> people)
        {
            return people.Select(p => p.ToResponse()).ToList();
        }

        private static List<AddressResponse> ToResponse(this List<Address> addresses)
        {
            return addresses.Select(add => new AddressResponse
            {
                AddressLine1 = add.AddressLine1,
                AddressLine2 = add.AddressLine2,
                AddressLine3 = add.AddressLine3,
                AddressLine4 = add.AddressLine4,
                PostCode = add.PostCode
            }).ToList();
        }
    }
}
