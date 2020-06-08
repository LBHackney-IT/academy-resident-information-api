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
                ResidentAddress = domain.ResidentAddress.ToResponse(),
            };
        }

        public static List<ResidentInformationResponse> ToResponse(this IEnumerable<ResidentInformation> people)
        {
            return people.Select(p => p.ToResponse()).ToList();
        }

        private static AddressResponse ToResponse(this Address residentAddress)
        {
            return new AddressResponse()
            {
                AddressLine1 = residentAddress.AddressLine1,
                AddressLine2 = residentAddress.AddressLine2,
                AddressLine3 = residentAddress.AddressLine3,
                AddressLine4 = residentAddress.AddressLine4,
                PostCode = residentAddress.PostCode
            };
        }
    }
}
