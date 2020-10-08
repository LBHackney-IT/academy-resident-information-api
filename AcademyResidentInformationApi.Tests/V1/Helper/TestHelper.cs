using System;
using System.Collections.Generic;
using System.Linq;
using AcademyResidentInformationApi.V1.Infrastructure;
using AutoFixture;
using Bogus;
using Address = AcademyResidentInformationApi.V1.Infrastructure.Address;
using Person = AcademyResidentInformationApi.V1.Infrastructure.Person;

namespace AcademyResidentInformationApi.Tests.V1.Helper
{
    public static class TestHelper
    {
        private static Faker _faker = new Faker();
        public static Person CreateDatabaseClaimantEntity(string firstname = null, string lastname = null, int? id = null,
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
            fp.ClaimId = id ?? _faker.Random.Number(0, 150);
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
            tp.AccountRef = accountRef ?? _faker.Random.Int(0);
            return tp;
        }

        public static CouncilProperty CreateDatabasePropertyForTaxPayer(string propertyRef, string address = null, string postcode = null)
        {
            var faker = new Fixture();
            var cp = faker.Build<CouncilProperty>()
                .With(p => p.PropertyRef, propertyRef)
                .Create();
            cp.AddressLine1 = address ?? cp.AddressLine1;
            cp.PostCode = postcode ?? cp.PostCode;
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

        public static Email CreateDatabaseEmailAddressForTaxPayer(int accountRef, string email = null)
        {
            var faker = new Fixture();
            var fakeEmail = faker.Build<Email>()
                .With(email => email.ReferenceId, accountRef)
                .Create();

            if (email == null) return fakeEmail;

            fakeEmail.EmailAddress = email;

            return fakeEmail;
        }

        public static PhoneNumber CreateDatabasePhoneNumbersForTaxPayer(int accountRef, List<string> phoneNumbers = null)
        {
            var faker = new Fixture();
            var fakePhone = faker.Build<PhoneNumber>()
                .With(fp => fp.Reference, accountRef.ToString())
                .Create();

            if (phoneNumbers == null) return fakePhone;

            fakePhone.Number1 = phoneNumbers.ElementAtOrDefault(0);
            fakePhone.Number2 = phoneNumbers.ElementAtOrDefault(1);
            fakePhone.Number3 = phoneNumbers.ElementAtOrDefault(2);
            fakePhone.Number4 = phoneNumbers.ElementAtOrDefault(3);

            return fakePhone;
        }
    }
}
