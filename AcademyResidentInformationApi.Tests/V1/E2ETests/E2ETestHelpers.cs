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
        public static ClaimantInformation AddPersonWithRelatesEntitiesToDb(AcademyContext context, string id = null, string firstname = null, string lastname = null)
        {
            var person = TestHelper.CreateDatabasePersonEntity();
            if (id != null)
            {
                var separateIdComponents = id.Split("-");
                person.Id = int.Parse(separateIdComponents[0]);
                person.PersonRef = int.Parse(separateIdComponents[1]);
            }
            person.FirstName = firstname ?? person.FirstName;
            person.LastName = lastname ?? person.LastName;

            var personEntity = context.Persons.Add(person);

            context.SaveChanges();

            var address = TestHelper.CreateDatabaseAddressForPersonId(personEntity.Entity.Id, personEntity.Entity.HouseId);


            context.Addresses.Add(address);
            context.SaveChanges();
            return new ClaimantInformation
            {
                AcademyId = $"{person.Id}-{person.PersonRef}",
                FirstName = person.FirstName,
                LastName = person.LastName,
                DateOfBirth = person.DateOfBirth.ToString("O", CultureInfo.InvariantCulture),
                NINumber = person.NINumber,
                AddressList = new List<Address>
                {
                    new Address {
                                AddressLine1 = address.AddressLine1,
                                AddressLine2 = address.AddressLine2,
                                AddressLine3 = address.AddressLine3,
                                AddressLine4 = address.AddressLine4,
                                PostCode = address.PostCode
                                }
                }

            };
        }
    }
}
