using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AcademyResidentInformationApi.Tests.V1.Helper;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Gateways;
using AcademyResidentInformationApi.V1.Infrastructure;
using FluentAssertions;
using NUnit.Framework;
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
        public void GetClaimantInformationByAcademyIdWhenThereAreNoMatchingRecordsReturnsNull()
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

            var address = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity.ClaimId, databaseEntity.HouseId);
            AcademyContext.Addresses.Add(address);
            AcademyContext.SaveChanges();

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
        public void GetAllClaimantsIfThereAreNoClaimantsReturnsAnEmptyList()
        {
            _classUnderTest.GetAllClaimants(0, 20).Should().BeEmpty();
        }

        [Test]
        public void GetAllClaimantsIfThereAreClaimantsWillReturnThem()
        {
            var databaseEntity = AddPersonRecordToDatabase();

            var listOfPersons = _classUnderTest.GetAllClaimants(0, 20);

            listOfPersons.Should().BeNullOrEmpty();
        }

        [Test]
        public void GetAllClaimantsIfThereAreClaimantAddressesWillReturnThem()
        {
            var databaseEntity = AddPersonRecordToDatabase();

            var address = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity.ClaimId, databaseEntity.HouseId);
            AcademyContext.Addresses.Add(address);
            AcademyContext.SaveChanges();

            var listOfPersons = _classUnderTest.GetAllClaimants(0, 20, postcode: address.PostCode);

            listOfPersons
                .First(p => ExtractClaimIdFromAcademyIdString(p.AcademyId).Equals(databaseEntity.ClaimId))
                .ClaimantAddress
                .Should().BeEquivalentTo(address.ToDomain());
        }

        [Test]
        public void GetAllResidentsWithFirstNameQueryParameterReturnsMatchingResident()
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

            var listOfPersons = _classUnderTest.GetAllClaimants(cursor: 0, limit: 20, firstname: "ciasom");
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

            var listOfPersons = _classUnderTest.GetAllClaimants(cursor: 0, limit: 20, lastname: "tessellate");
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
            var firstPersonId = ExtractClaimIdFromAcademyIdString(listOfPersons.First().AcademyId);

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
                .First(p => ExtractClaimIdFromAcademyIdString(p.AcademyId).Equals(databaseEntity.ClaimId))
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
            var firstPersonId = ExtractClaimIdFromAcademyIdString(listOfPersons.First().AcademyId);

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

            var address = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity.ClaimId, databaseEntity.HouseId, postcode);
            AcademyContext.Addresses.Add(address);
            AcademyContext.SaveChanges();

            var listOfPersons = _classUnderTest.GetAllClaimants(cursor: 0, limit: 20, postcode: "E8 1DY");
            var firstPersonId = ExtractClaimIdFromAcademyIdString(listOfPersons.First().AcademyId);

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

            var address = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity.ClaimId, databaseEntity.HouseId, address: "1 My Street, Hackney, London");
            AcademyContext.Addresses.Add(address);
            AcademyContext.SaveChanges();

            var address1 = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity1.ClaimId, databaseEntity.HouseId, address: "5 Another Street, Lambeth, London");
            AcademyContext.Addresses.Add(address1);
            AcademyContext.SaveChanges();

            var listOfPersons = _classUnderTest.GetAllClaimants(cursor: 0, limit: 20, address: addressQuery).ToList();
            listOfPersons.Count.Should().Be(1);
            listOfPersons
                .First(p => ExtractClaimIdFromAcademyIdString(p.AcademyId).Equals(databaseEntity.ClaimId))
                .ClaimantAddress
                .Should().BeEquivalentTo(address.ToDomain());
        }
        private Person AddPersonRecordToDatabase(string firstname = null, string lastname = null, int? id = null)
        {
            var databaseEntity = TestHelper.CreateDatabasePersonEntity(firstname, lastname, id);
            AcademyContext.Persons.Add(databaseEntity);
            AcademyContext.SaveChanges();
            return databaseEntity;
        }

        private static int ExtractClaimIdFromAcademyIdString(string academyId)
        {
            return int.Parse(academyId.Split('-').First());
        }
    }
}
