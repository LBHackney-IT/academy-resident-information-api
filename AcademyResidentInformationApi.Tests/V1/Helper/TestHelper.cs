using System;
using AcademyResidentInformationApi.V1.Infrastructure;
using AutoFixture;
using Address = AcademyResidentInformationApi.V1.Infrastructure.Address;

namespace AcademyResidentInformationApi.Tests.V1.Helper
{
    public static class TestHelper
    {
        public static Person CreateDatabasePersonEntity(string firstname = null, string lastname = null, int? id = null,
            int? memberId = null, int? personRef = null, int? houseId = null)
        {
            var faker = new Fixture();
            var fp = faker.Build<Person>()
                .Without(p => p.Address)
                .Without(p => p.Claim)
                .Create();
            fp.DateOfBirth = faker.Create<DateTime>().ToString("yyyy-MM-dd");
            fp.FirstName = firstname ?? fp.FirstName;
            fp.LastName = lastname ?? fp.LastName;
            fp.ClaimId = id ?? fp.ClaimId;
            fp.MemberId = memberId ?? fp.MemberId;
            fp.PersonRef = personRef ?? fp.PersonRef;
            fp.HouseId = houseId ?? fp.HouseId;
            return fp;
        }

        public static Address CreateDatabaseAddressForPersonId(int? claimId, int? houseId, string postcode = null,
            string address = null, string toDate = "2099-12-31 00:00:00.0000000")
        {
            var faker = new Fixture();

            var fa = faker.Build<Address>()
                .With(add => add.ClaimId, claimId)
                .With(add => add.HouseId, houseId)
                .With(add => add.ToDate, toDate)
                .Without(add => add.Claim)
                .Without(add => add.Person)
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

        public static TaxPayer CreateDatabaseTaxPayerEntity(int? accountRef, string firstname = null, string lastname = null)
        {
            var faker = new Fixture();
            var tp = faker.Build<TaxPayer>()
                .Create();
            tp.FirstName = firstname ?? tp.FirstName;
            tp.LastName = lastname ?? tp.LastName;
            tp.AccountRef = accountRef ?? tp.AccountRef;
            return tp;
        }

        public static CouncilProperty CreateDatabasePropertyForTaxPayer(string propertyRef)
        {
            var faker = new Fixture();
            var cp = faker.Build<CouncilProperty>()
                .With(p => p.PropertyRef, propertyRef)
                .Create();
            return cp;
        }

        public static Occupation CreateDatabaseOccupationEntityForCouncilProperty(int accountRef)
        {
            var faker = new Fixture();
            var cto = faker.Build<Occupation>()
                .With(o => o.AccountRef, accountRef)
                .Create();
            return cto;
        }
    }
}
