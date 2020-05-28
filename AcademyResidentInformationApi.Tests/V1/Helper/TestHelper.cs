using System;
using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Domain;
using AcademyResidentInformationApi.V1.Infrastructure;
using AutoFixture;
using Address = AcademyResidentInformationApi.V1.Infrastructure.Address;

namespace AcademyResidentInformationApi.Tests.V1.Helper
{
    public static class TestHelper
    {
        public static ResidentInformation CreateDomainResident()
        {
            var faker = new Fixture();
            return faker.Create<ResidentInformation>();
        }

        public static Person CreateDatabasePersonEntity()
        {
            var faker = new Fixture();
            var fp = faker.Create<Person>();
            fp.DateOfBirth = new DateTime
                (fp.DateOfBirth.Year, fp.DateOfBirth.Month, fp.DateOfBirth.Day);
            return fp;
        }

        public static Address CreateDatabaseAddressForPersonId(int claimId)
        {
            var faker = new Fixture();

            return faker.Build<Address>()
                .With(add => add.ClaimId, claimId)
                .Without(add => add.Person)
                .Create();
        }
    }
}
