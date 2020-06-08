using System;
using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Domain;
using AcademyResidentInformationApi.V1.Infrastructure;
using AutoFixture;
using Address = AcademyResidentInformationApi.V1.Infrastructure.Address;
using ResidentInformation = AcademyResidentInformationApi.V1.Domain.ResidentInformation;

namespace AcademyResidentInformationApi.Tests.V1.Helper
{
    public static class TestHelper
    {
        public static ResidentInformation CreateDomainResident()
        {
            var faker = new Fixture();
            return faker.Create<ResidentInformation>();
        }

        public static Person CreateDatabasePersonEntity(string firstname = null, string lastname = null, int? id = null)
        {
            var faker = new Fixture();
            var fp = faker.Create<Person>();
            fp.DateOfBirth = new DateTime
                (fp.DateOfBirth.Year, fp.DateOfBirth.Month, fp.DateOfBirth.Day);
            fp.FirstName = firstname ?? fp.FirstName;
            fp.LastName = lastname ?? fp.LastName;
            fp.Id = id ?? fp.Id;
            return fp;
        }

        public static Address CreateDatabaseAddressForPersonId(int claimId, string postcode = null, string address = null)
        {
            var faker = new Fixture();

            var fa = faker.Build<Address>()
                .With(add => add.ClaimId, claimId)
                .Without(add => add.Person)
                .Create();

            fa.PostCode = postcode ?? fa.PostCode;
            fa.AddressLine1 = address ?? fa.AddressLine1;
            return fa;
        }
    }
}
