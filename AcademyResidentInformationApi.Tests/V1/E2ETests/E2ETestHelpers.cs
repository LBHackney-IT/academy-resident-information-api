using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AcademyResidentInformationApi.Tests.V1.Helper;
using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Infrastructure;
using Address = AcademyResidentInformationApi.V1.Boundary.Responses.Address;

namespace AcademyResidentInformationApi.Tests.V1.E2ETests
{
    public static class E2ETestHelpers
    {
        public static ClaimantInformation AddPersonWithRelatesEntitiesToDb(AcademyContext context, int? claimid = null, int? personref = null, string firstname = null, string lastname = null, string postcode = null, string addressLines = null)
        {
            var person = TestHelper.CreateDatabasePersonEntity();
            person.ClaimId = claimid ?? person.ClaimId;
            person.PersonRef = personref ?? person.PersonRef;

            person.FirstName = firstname ?? person.FirstName;
            person.LastName = lastname ?? person.LastName;

            var personEntity = context.Persons.Add(person);

            context.SaveChanges();

            var address = TestHelper.CreateDatabaseAddressForPersonId(personEntity.Entity.ClaimId, personEntity.Entity.HouseId, address: addressLines, postcode: postcode);


            context.Addresses.Add(address);
            context.SaveChanges();
            return new ClaimantInformation
            {
                ClaimId = person.ClaimId,
                PersonRef = person.PersonRef,
                FirstName = person.FirstName,
                LastName = person.LastName,
                DateOfBirth = person.DateOfBirth.ToString("O", CultureInfo.InvariantCulture),
                NINumber = person.NINumber,
                ClaimantAddress = new Address
                {
                    AddressLine1 = address.AddressLine1,
                    AddressLine2 = address.AddressLine2,
                    AddressLine3 = address.AddressLine3,
                    AddressLine4 = address.AddressLine4,
                    PostCode = address.PostCode
                }

            };
        }
    }
}
