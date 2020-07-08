using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AcademyResidentInformationApi.Tests.V1.Helper;
using AcademyResidentInformationApi.V1.Domain;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Gateways;
using AcademyResidentInformationApi.V1.Infrastructure;
using FluentAssertions;
using NUnit.Framework;
using Address = AcademyResidentInformationApi.V1.Infrastructure.Address;
using DomainAddress = AcademyResidentInformationApi.V1.Domain.Address;

namespace AcademyResidentInformationApi.Tests.V1.Gateways
{
    [TestFixture]
    public class AcademyGatewayTests : DatabaseTests
    {
        private AcademyGateway _classUnderTest;

        [SetUp]
        public new void Setup()
        {
            _classUnderTest = new AcademyGateway(AcademyContext);
        }

        [Test]
        public void GatewayImplementsBoundaryInterface()
        {
            Assert.NotNull(_classUnderTest is IAcademyGateway);
        }

        [Test]
        public void GetClaimantInformationByClaimIdAndPersonRefWhenThereAreNoMatchingRecordsReturnsNull()
        {
            var response = _classUnderTest.GetClaimantById(123, 456);
            response.Should().BeNull();
        }

        [Test]
        public void GetClaimantInformationByClaimIdAndPersonRefReturnsPersonalDetails()
        {
            var databaseEntity = AddPersonRecordToDatabase();
            var response = _classUnderTest.GetClaimantById(databaseEntity.ClaimId, databaseEntity.PersonRef);

            response.FirstName.Should().Be(databaseEntity.FirstName);
            response.LastName.Should().Be(databaseEntity.LastName);
            response.NINumber.Should().Be(databaseEntity.NINumber);
            response.DateOfBirth.Should().Be(databaseEntity.DateOfBirth.ToString("O"));
            response.Should().NotBe(null);
        }

        [Test]
        public void GetClaimantInformationByClaimIdAndPersonRefReturnsAddressDetails()
        {
            var databaseEntity = AddPersonRecordToDatabase();
            var address = AddAddressToDatabase(databaseEntity.ClaimId, databaseEntity.HouseId);

            var response = _classUnderTest.GetClaimantById(databaseEntity.ClaimId, databaseEntity.PersonRef);

            var expectedDomainAddress = new DomainAddress
            {
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                AddressLine3 = address.AddressLine3,
                AddressLine4 = address.AddressLine4,
                PostCode = address.PostCode,
            };
            response.ClaimantAddress.Should().BeEquivalentTo(expectedDomainAddress);
        }

        [Test]
        public void GetClaimantInformationByClaimIdAndPersonRefReturnsCheckDigit()
        {
            var personEntity = AddPersonRecordToDatabase(withClaim: false);
            var claimEntity = AddClaimToDatabase(personEntity.ClaimId);

            var response = _classUnderTest.GetClaimantById(personEntity.ClaimId, personEntity.PersonRef);

            response.CheckDigit.Should().Be(claimEntity.CheckDigit);
        }

        [Test]
        public void GetAllClaimantsIfThereAreNoClaimantsReturnsAnEmptyList()
        {
            _classUnderTest.GetAllClaimants(0, 20).Should().BeEmpty();
        }

        [Test]
        public void GetAllClaimantsWillReturnThemWithCheckDigit()
        {
            var personEntity = AddPersonRecordToDatabase(withClaim: false);
            var claim = AddClaimToDatabase(personEntity.ClaimId);
            AddAddressToDatabase(personEntity.ClaimId, personEntity.HouseId);

            var expectedDomain = personEntity.ToDomain();
            expectedDomain.CheckDigit = claim.CheckDigit;

            var personFromList = _classUnderTest.GetAllClaimants(0, 20)
                .First(p => p.ClaimId == personEntity.ClaimId);

            personFromList.CheckDigit.Should().Be(claim.CheckDigit);
        }

        [Test]
        public void GetAllClaimantsIfThereAreClaimantWillReturnThemWithAddresses()
        {
            var databaseEntity = AddPersonRecordToDatabase();
            var address = AddAddressToDatabase(databaseEntity.ClaimId, databaseEntity.HouseId);

            var domainEntity = databaseEntity.ToDomain();
            domainEntity.ClaimantAddress = address.ToDomain();

            var listOfPersons = _classUnderTest.GetAllClaimants(0, 20, postcode: address.PostCode);

            listOfPersons
                .First(p => p.ClaimId == databaseEntity.ClaimId)
                .ClaimantAddress
                .Should().BeEquivalentTo(address.ToDomain());
        }

        [Test]
        public void GetAllResidentsWithFirstNameQueryParameterReturnsMatchingResident()
        {
            var databaseEntity = AddPersonRecordToDatabase(firstname: "ciasom");
            var databaseEntity1 = AddPersonRecordToDatabase(firstname: "shape");
            var databaseEntity2 = AddPersonRecordToDatabase(firstname: "Ciasom");

            var address = AddAddressToDatabase(databaseEntity.ClaimId, databaseEntity.HouseId);
            var address1 = AddAddressToDatabase(databaseEntity1.ClaimId, databaseEntity1.HouseId);
            var address2 = AddAddressToDatabase(databaseEntity2.ClaimId, databaseEntity2.HouseId);

            var domainEntity = databaseEntity.ToDomain();
            domainEntity.ClaimantAddress = address.ToDomain();

            var domainEntity2 = databaseEntity2.ToDomain();
            domainEntity2.ClaimantAddress = address2.ToDomain();

            var listOfPersons = _classUnderTest.GetAllClaimants(cursor: 0, limit: 20, firstname: "ciasom");
            listOfPersons.Count.Should().Be(2);
            listOfPersons.Should().ContainEquivalentOf(domainEntity);
            listOfPersons.Should().ContainEquivalentOf(domainEntity2);

        }

        [Test]
        public void GetAllResidentsWildcardSearchWithFirstNameQueryParameterReturnsMatchingResident()
        {
            var databaseEntity = AddPersonRecordToDatabase(firstname: "ciasom");
            var databaseEntity1 = AddPersonRecordToDatabase(firstname: "shape");
            var databaseEntity2 = AddPersonRecordToDatabase(firstname: "Ciasom");

            var address = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity.ClaimId, databaseEntity.HouseId);
            AcademyContext.Addresses.Add(address);
            AcademyContext.SaveChanges();

            var address1 = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity1.ClaimId, databaseEntity1.HouseId);
            AcademyContext.Addresses.Add(address1);
            AcademyContext.SaveChanges();

            var address2 = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity2.ClaimId, databaseEntity2.HouseId);
            AcademyContext.Addresses.Add(address2);
            AcademyContext.SaveChanges();

            var domainEntity = databaseEntity.ToDomain();
            domainEntity.ClaimantAddress = address.ToDomain();

            var domainEntity2 = databaseEntity2.ToDomain();
            domainEntity2.ClaimantAddress = address2.ToDomain();

            var listOfPersons = _classUnderTest.GetAllClaimants(cursor: 0, limit: 20, firstname: "iaso");
            listOfPersons.Count.Should().Be(2);
            listOfPersons.Should().ContainEquivalentOf(domainEntity);
            listOfPersons.Should().ContainEquivalentOf(domainEntity2);

        }

        [Test]
        public void GetAllResidentsWithLastNameQueryParameterReturnsMatchingResident()
        {
            var databaseEntity = AddPersonRecordToDatabase(lastname: "tessellate");
            var databaseEntity1 = AddPersonRecordToDatabase(lastname: "square");
            var databaseEntity2 = AddPersonRecordToDatabase(lastname: "Tessellate");

            var address = AddAddressToDatabase(databaseEntity.ClaimId, databaseEntity.HouseId);
            var address1 = AddAddressToDatabase(databaseEntity1.ClaimId, databaseEntity1.HouseId);
            var address2 = AddAddressToDatabase(databaseEntity2.ClaimId, databaseEntity2.HouseId);

            var domainEntity = databaseEntity.ToDomain();
            domainEntity.ClaimantAddress = address.ToDomain();

            var domainEntity2 = databaseEntity2.ToDomain();
            domainEntity2.ClaimantAddress = address2.ToDomain();

            var listOfPersons = _classUnderTest.GetAllClaimants(cursor: 0, limit: 20, lastname: "tessellate");
            listOfPersons.Count.Should().Be(2);
            listOfPersons.Should().ContainEquivalentOf(domainEntity);
            listOfPersons.Should().ContainEquivalentOf(domainEntity2);
        }

        [Test]
        public void GetAllResidentsWildcardSearchWithLastNameQueryParameterReturnsMatchingResident()
        {
            var databaseEntity = AddPersonRecordToDatabase(lastname: "tessellate");
            var databaseEntity1 = AddPersonRecordToDatabase(lastname: "square");
            var databaseEntity2 = AddPersonRecordToDatabase(lastname: "Tessellate");

            var address = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity.ClaimId, databaseEntity.HouseId);
            AcademyContext.Addresses.Add(address);
            AcademyContext.SaveChanges();

            var address1 = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity1.ClaimId, databaseEntity1.HouseId);
            AcademyContext.Addresses.Add(address1);
            AcademyContext.SaveChanges();

            var address2 = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity2.ClaimId, databaseEntity2.HouseId);
            AcademyContext.Addresses.Add(address2);
            AcademyContext.SaveChanges();

            var domainEntity = databaseEntity.ToDomain();
            domainEntity.ClaimantAddress = address.ToDomain();

            var domainEntity2 = databaseEntity2.ToDomain();
            domainEntity2.ClaimantAddress = address2.ToDomain();

            var listOfPersons = _classUnderTest.GetAllClaimants(cursor: 0, limit: 20, lastname: "sell");
            listOfPersons.Count.Should().Be(2);
            listOfPersons.Should().ContainEquivalentOf(domainEntity);
            listOfPersons.Should().ContainEquivalentOf(domainEntity2);
        }

        [Test]
        public void GetAllResidentsWithNameQueryParametersReturnsMatchingResidentOnlyOnce()
        {
            var databaseEntity = AddPersonRecordToDatabase(firstname: "ciasom", lastname: "Tessellate");

            var address = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity.ClaimId, databaseEntity.HouseId);
            AcademyContext.Addresses.Add(address);
            AcademyContext.SaveChanges();

            var listOfPersons = _classUnderTest.GetAllClaimants(cursor: 0, limit: 20, firstname: "ciasom", lastname: "Tessellate");
            var firstPersonId = listOfPersons.First().ClaimId;

            listOfPersons.Count.Should().Be(1);
            firstPersonId.Should().Be(databaseEntity.ClaimId);
        }

        [Test]
        public void GetAllResidentsWildcardSearchWithNameQueryParametersReturnsMatchingResidentOnlyOnce()
        {
            var databaseEntity = AddPersonRecordToDatabase(firstname: "ciasom", lastname: "Tessellate");

            var address = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity.ClaimId, databaseEntity.HouseId);
            AcademyContext.Addresses.Add(address);
            AcademyContext.SaveChanges();

            var listOfPersons = _classUnderTest.GetAllClaimants(cursor: 0, limit: 20, firstname: "ciaso", lastname: "essellat");
            var firstPersonId = listOfPersons.First().ClaimId;

            listOfPersons.Count.Should().Be(1);
            firstPersonId.Should().Be(databaseEntity.ClaimId);
        }

        [Test]
        public void GetAllResidentsWithPostCodeQueryParameterReturnsMatchingResident()
        {
            var databaseEntity = AddPersonRecordToDatabase();
            var databaseEntity1 = AddPersonRecordToDatabase();

            var address = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity.ClaimId, databaseEntity.HouseId, "E8 1DY");
            AcademyContext.Addresses.Add(address);
            AcademyContext.SaveChanges();

            var address1 = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity1.ClaimId, databaseEntity.HouseId, "E8 5TG");
            AcademyContext.Addresses.Add(address1);
            AcademyContext.SaveChanges();

            var listOfPersons = _classUnderTest.GetAllClaimants(cursor: 0, limit: 20, postcode: "E8 1DY");
            listOfPersons.Count.Should().Be(1);
            listOfPersons
                .First(p => p.ClaimId == databaseEntity.ClaimId)
                .ClaimantAddress
                .Should().BeEquivalentTo(address.ToDomain());
        }

        [Test]
        public void GetAllResidentsWithNameAndPostCodeQueryParameterReturnsMatchingResident()
        {
            var databaseEntity = AddPersonRecordToDatabase(firstname: "ciasom");
            var address = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity.ClaimId, databaseEntity.HouseId, "E8 1DY");
            AcademyContext.Addresses.Add(address);
            AcademyContext.SaveChanges();

            var databaseEntity1 = AddPersonRecordToDatabase(firstname: "wrong name");
            var address1 = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity1.ClaimId, databaseEntity1.HouseId, "E8 1DY");
            AcademyContext.Addresses.Add(address1);
            AcademyContext.SaveChanges();

            var databaseEntity2 = AddPersonRecordToDatabase(firstname: "ciasom");
            var address2 = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity2.ClaimId, databaseEntity2.HouseId, "E8 5RT");
            AcademyContext.Addresses.Add(address2);
            AcademyContext.SaveChanges();

            var listOfPersons = _classUnderTest.GetAllClaimants(cursor: 0, limit: 20, firstname: "ciasom", postcode: "E8 1DY").ToList();
            var firstPersonId = listOfPersons.First().ClaimId;

            listOfPersons.Count.Should().Be(1);
            firstPersonId.Should().Be(databaseEntity.ClaimId);
            listOfPersons.First()
                .ClaimantAddress
                .Should().BeEquivalentTo(address.ToDomain());
        }

        [TestCase("E81DY")]
        [TestCase("e8 1DY")]
        public void GetAllResidentsWithPostCodeQueryParameterIgnoresFormatting(string postcode)
        {
            var databaseEntity = AddPersonRecordToDatabase();
            var address = AddAddressToDatabase(databaseEntity.ClaimId, databaseEntity.HouseId, postcode: postcode);

            var listOfPersons = _classUnderTest.GetAllClaimants(cursor: 0, limit: 20, postcode: "E8 1DY");
            var firstPersonId = listOfPersons.First().ClaimId;

            listOfPersons.Count.Should().Be(1);

            firstPersonId.Should().Be(databaseEntity.ClaimId);
            listOfPersons.First().ClaimantAddress
                .Should().BeEquivalentTo(address.ToDomain());
        }

        [TestCase("1 My Street")]
        [TestCase("My Street")]
        [TestCase("1 My Street, Hackney, London")]
        [TestCase("Hackney")]
        public void GetAllResidentsWithAddressQueryParameterReturnsMatchingResident(string addressQuery)
        {
            var databaseEntity = AddPersonRecordToDatabase();
            var databaseEntity1 = AddPersonRecordToDatabase();

            var address = AddAddressToDatabase(databaseEntity.ClaimId, databaseEntity.HouseId, address: "1 My Street, Hackney, London");
            var address1 = AddAddressToDatabase(databaseEntity1.ClaimId, databaseEntity.HouseId, address: "5 Another Street, Lambeth, London");

            var listOfPersons = _classUnderTest.GetAllClaimants(cursor: 0, limit: 20, address: addressQuery).ToList();
            listOfPersons.Count.Should().Be(1);
            listOfPersons
                .First(p => p.ClaimId == databaseEntity.ClaimId)
                .ClaimantAddress
                .Should().BeEquivalentTo(address.ToDomain());
        }

        [Test]
        public void GetAllResidentsWontReturnMoreRecordsThanTheLimit()
        {
            var manyPeople = new List<Person>
            {
                AddPersonRecordToDatabase(),
                AddPersonRecordToDatabase(),
                AddPersonRecordToDatabase()
            }.OrderBy(p => p.ClaimId).ToList();
            manyPeople.ForEach(p => AddAddressToDatabase(p.ClaimId, p.HouseId));

            var peopleReturned = _classUnderTest.GetAllClaimants(0, 2);
            peopleReturned.Count.Should().Be(2);
        }

        [Test]
        public void GetAllResidentsWillOffsetRecordsByTheCursor()
        {
            var manyPeople = new List<Person>
            {
                AddPersonRecordToDatabase(),
                AddPersonRecordToDatabase(),
                AddPersonRecordToDatabase()
            }.OrderBy(p => p.ClaimId).ToList();
            manyPeople.ForEach(p => AddAddressToDatabase(p.ClaimId, p.HouseId));

            var peopleReturned = _classUnderTest.GetAllClaimants(1, 2);
            peopleReturned.Count.Should().Be(2);
            peopleReturned.Should().Contain(ci => ci.ClaimId == manyPeople.ElementAt(1).ClaimId);
            peopleReturned.Should().Contain(ci => ci.ClaimId == manyPeople.ElementAt(2).ClaimId);
        }
        private Person AddPersonRecordToDatabase(string firstname = null, string lastname = null, int? id = null, bool withClaim = true)
        {
            var databaseEntity = TestHelper.CreateDatabasePersonEntity(firstname, lastname, id);
            AcademyContext.Persons.Add(databaseEntity);
            AcademyContext.SaveChanges();
            if (withClaim)
            {
                AcademyContext.Claims.Add(new Claim { ClaimId = databaseEntity.ClaimId });
                AcademyContext.SaveChanges();
            }
            return databaseEntity;
        }

        private Address AddAddressToDatabase(int claimId, int houseId, string address = null, string postcode = null)
        {
            var addressEntity = TestHelper.CreateDatabaseAddressForPersonId(claimId, houseId, postcode, address);
            AcademyContext.Addresses.Add(addressEntity);
            AcademyContext.SaveChanges();
            return addressEntity;
        }

        private Claim AddClaimToDatabase(int claimId)
        {
            var claimEntity = TestHelper.CreateDatabaseClaimEntity(claimId);
            AcademyContext.Claims.Add(claimEntity);
            AcademyContext.SaveChanges();
            return claimEntity;
        }
    }
}
