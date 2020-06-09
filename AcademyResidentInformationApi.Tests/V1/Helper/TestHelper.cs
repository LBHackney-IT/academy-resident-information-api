using System;
using AcademyResidentInformationApi.V1.Boundary.Responses;
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

        public static Person CreateDatabasePersonEntity()
        {
            var faker = new Fixture();
            var fp = faker.Build<Person>().Without(p => p.Address).Create();
            fp.DateOfBirth = new DateTime
                (fp.DateOfBirth.Year, fp.DateOfBirth.Month, fp.DateOfBirth.Day);
            return fp;
        }

        public static Address CreateDatabaseAddressForPersonId(int claimId, int houseId)
        {
            var faker = new Fixture();

            return faker.Build<Address>()
                .With(add => add.ClaimId, claimId)
                .With(add => add.HouseId, houseId)
                .Without(add => add.Person)
                .Create();
        }
    }
}
