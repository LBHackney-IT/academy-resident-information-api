using System;
using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Infrastructure;
using AutoFixture;
using Address = AcademyResidentInformationApi.V1.Infrastructure.Address;
using ClaimantInformation = AcademyResidentInformationApi.V1.Domain.ClaimantInformation;

namespace AcademyResidentInformationApi.Tests.V1.Helper
{
    public static class TestHelper
    {
        public static Person CreateDatabasePersonEntity(string firstname = null, string lastname = null, int? id = null)
        {
            var faker = new Fixture();
            var fp = faker.Build<Person>()
                .Without(p => p.Address)
                .Without(p => p.Claim)
                .Create();
            fp.DateOfBirth = new DateTime
                (fp.DateOfBirth.Year, fp.DateOfBirth.Month, fp.DateOfBirth.Day);
            fp.FirstName = firstname ?? fp.FirstName;
            fp.LastName = lastname ?? fp.LastName;
            fp.ClaimId = id ?? fp.ClaimId;
            return fp;
        }

        public static Address CreateDatabaseAddressForPersonId(int claimId, int houseId, string postcode = null, string address = null)
        {
            var faker = new Fixture();

            var fa = faker.Build<Address>()
                .With(add => add.ClaimId, claimId)
                .With(add => add.HouseId, houseId)
                .Without(add => add.Person)
                .Without(add => add.Claim)
                .Create();

            fa.PostCode = postcode ?? fa.PostCode;
            fa.AddressLine1 = address ?? fa.AddressLine1;
            return fa;
        }

        public static Claim CreateDatabaseClaimEntity(int? claimId)
        {
            var fixture = new Fixture();

            var claim = new Claim
            {
                ClaimId = claimId ?? fixture.Create<int>(),
                CheckDigit = fixture.Create<char>().ToString()
            };
            return claim;
        }
    }
}
