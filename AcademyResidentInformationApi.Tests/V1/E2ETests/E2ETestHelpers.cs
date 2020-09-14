using System.Collections.Generic;
using AcademyResidentInformationApi.Tests.V1.Helper;
using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Infrastructure;
using Address = AcademyResidentInformationApi.V1.Boundary.Responses.Address;

namespace AcademyResidentInformationApi.Tests.V1.E2ETests
{
    public static class E2ETestHelpers
    {
        public static ClaimantInformation AddClaimantWithRelatesEntitiesToDb(AcademyContext context, int? claimId = null, int? personRef = null, string firstname = null, string lastname = null, string postcode = null, string addressLines = null)
        {
            var person = TestHelper.CreateDatabaseClaimantEntity();
            person.ClaimId = claimId ?? person.ClaimId;
            person.PersonRef = personRef ?? person.PersonRef;

            person.FirstName = firstname ?? person.FirstName;
            person.LastName = lastname ?? person.LastName;

            var personEntity = context.Persons.Add(person);
            context.SaveChanges();

            var address = TestHelper.CreateDatabaseAddressForPersonId(personEntity.Entity.ClaimId,
                personEntity.Entity.HouseId, address: addressLines, postcode: postcode);
            context.Addresses.Add(address);
            context.SaveChanges();

            var claim = TestHelper.CreateDatabaseClaimEntity(personEntity.Entity.ClaimId);
            context.Claims.Add(claim);
            context.SaveChanges();

            return new ClaimantInformation
            {
                ClaimId = person.ClaimId.Value,
                CheckDigit = claim.CheckDigit,
                PersonRef = person.PersonRef.Value,
                Title = person.Title,
                FirstName = person.FirstName,
                LastName = person.LastName,
                DateOfBirth = person.DateOfBirth,
                NINumber = person.NINumber,
                StatusIndicator = claim.StatusIndicator,
                ClaimantAddress = new Address
                {
                    AddressLine1 = address.AddressLine1,
                    AddressLine2 = address.AddressLine2,
                    AddressLine3 = address.AddressLine3,
                    AddressLine4 = address.AddressLine4,
                    Postcode = address.PostCode
                }

            };
        }

        public static TaxPayerInformationResponse AddTaxPayerWithRelatesEntitiesToDb(AcademyContext context,
            int? accountRef = null, string firstname = null, string lastname = null,
            string postcode = null, string addressLines = null)
        {
            var person = TestHelper.CreateDatabaseTaxPayerEntity(accountRef, firstname, lastname);
            person.AccountRef = accountRef ?? person.AccountRef;

            person.FirstName = firstname ?? person.FirstName;
            person.LastName = lastname ?? person.LastName;

            context.TaxPayers.Add(person);
            context.SaveChanges();

            var occupation = TestHelper.CreateDatabaseOccupationEntityForCouncilProperty(person.AccountRef);
            context.Occupations.Add(occupation);
            context.SaveChanges();

            var property = TestHelper.CreateDatabasePropertyForTaxPayer(occupation.PropertyRef);
            context.CouncilProperties.Add(property);
            context.SaveChanges();

            var email = TestHelper.CreateDatabaseEmailAddressForTaxPayer(person.AccountRef);
            context.Emails.Add(email);
            context.SaveChanges();

            var phoneNumber = TestHelper.CreateDatabasePhoneNumbersForTaxPayer(person.AccountRef);
            context.PhoneNumbers.Add(phoneNumber);
            context.SaveChanges();

            return new TaxPayerInformationResponse()
            {
                AccountRef = person.AccountRef,
                FirstName = person.FirstName,
                LastName = person.LastName,
                EmailList = new List<string>{ email.EmailAddress },
                PhoneNumberList = new List<string>{ phoneNumber.Number1, phoneNumber.Number2, phoneNumber.Number3, phoneNumber.Number4 },
                TaxPayerAddress = new Address
                {
                    AddressLine1 = property.AddressLine1,
                    AddressLine2 = property.AddressLine2,
                    AddressLine3 = property.AddressLine3,
                    AddressLine4 = property.AddressLine4,
                    Postcode = property.PostCode
                },
                UPRN = property.Uprn
            };
        }
    }
}
