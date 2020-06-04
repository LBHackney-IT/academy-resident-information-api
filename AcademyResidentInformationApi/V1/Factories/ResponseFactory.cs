using System.Collections.Generic;
using System.Linq;
using AcademyResidentInformationApi.V1.Domain;
using AddressResponse = AcademyResidentInformationApi.V1.Boundary.Responses.Address;
using ResidentInformationResponse = AcademyResidentInformationApi.V1.Boundary.Responses.ResidentInformation;


namespace AcademyResidentInformationApi.V1.Factories
{
    public static class ResponseFactory
    {
        public static ResidentInformationResponse ToResponse(this ResidentInformation domain)
        {
            return new ResidentInformationResponse
            {
                FirstName = domain.FirstName,
                LastName = domain.LastName,
                DateOfBirth = domain.DateOfBirth,
                NINumber = domain.NINumber,
                AddressList = domain.AddressList.ToResponse(),
            };
        }

        public static List<ResidentInformationResponse> ToResponse(this IEnumerable<ResidentInformation> people)
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
