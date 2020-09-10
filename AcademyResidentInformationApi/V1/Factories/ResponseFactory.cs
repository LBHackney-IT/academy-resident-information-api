using System.Collections.Generic;
using System.Linq;
using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Domain;
using Address = AcademyResidentInformationApi.V1.Domain.Address;
using AddressResponse = AcademyResidentInformationApi.V1.Boundary.Responses.Address;
using ClaimantInformation = AcademyResidentInformationApi.V1.Domain.ClaimantInformation;
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
                Title = domain.Title,
                FirstName = domain.FirstName,
                LastName = domain.LastName,
                DateOfBirth = domain.DateOfBirth,
                NINumber = domain.NINumber,
                StatusIndicator = domain.StatusIndicator,
                ClaimantAddress = domain.ClaimantAddress.ToResponse(),
            };
        }

        public static List<ClaimantInformationResponse> ToResponse(this IEnumerable<ClaimantInformation> people)
        {
            return people.Select(p => p.ToResponse()).ToList();
        }

        public static TaxPayerInformationResponse ToResponse(this TaxPayerInformation taxPayerInfo)
        {
            return new TaxPayerInformationResponse
            {
                AccountRef = taxPayerInfo.AccountRef,
                UPRN = taxPayerInfo.Uprn,
                EmailList = taxPayerInfo.EmailList,
                PhoneNumberList = taxPayerInfo.PhoneNumberList,
                FirstName = taxPayerInfo.FirstName,
                LastName = taxPayerInfo.LastName,
                TaxPayerAddress = taxPayerInfo.TaxPayerAddress.ToResponse()
            };
        }

        private static AddressResponse ToResponse(this Address domainAddress)
        {
            return new AddressResponse()
            {
                AddressLine1 = domainAddress.AddressLine1,
                AddressLine2 = domainAddress.AddressLine2,
                AddressLine3 = domainAddress.AddressLine3,
                AddressLine4 = domainAddress.AddressLine4,
                Postcode = domainAddress.PostCode
            };
        }
    }
}
