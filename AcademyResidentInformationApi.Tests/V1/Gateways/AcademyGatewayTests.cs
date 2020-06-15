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
using DomainAddress = AcademyResidentInformationApi.V1.Domain.Address;

namespace AcademyResidentInformationApi.Tests.V1.Gateways
{
    [TestFixture]
    public class AcademyGatewayTests : DatabaseTests
    {
        // private Fixture _fixture = new Fixture();
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
            var response = _classUnderTest.GetClaimantById(databaseEntity.Id, databaseEntity.PersonRef);

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

            var address = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity.Id, databaseEntity.HouseId);
            AcademyContext.Addresses.Add(address);
            AcademyContext.SaveChanges();

            var response = _classUnderTest.GetClaimantById(databaseEntity.Id, databaseEntity.PersonRef);

            var expectedDomainAddress = new DomainAddress
            {
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                AddressLine3 = address.AddressLine3,
                AddressLine4 = address.AddressLine4,
                PostCode = address.PostCode,
            };
            response.AddressList.Should().BeEquivalentTo(new List<DomainAddress> { expectedDomainAddress });
        }

        [Test]
        public void GetAllClaimantsIfThereAreNoClaimantsReturnsAnEmptyList()
        {
            _classUnderTest.GetAllClaimants().Should().BeEmpty();
        }

        [Test]
        public void GetAllClaimantsIfThereAreClaimantsWillReturnThem()
        {
            var databaseEntity = AddPersonRecordToDatabase();
            var databaseEntity1 = AddPersonRecordToDatabase();
            var databaseEntity2 = AddPersonRecordToDatabase();

            var listOfPersons = _classUnderTest.GetAllClaimants();

            listOfPersons.Should().ContainEquivalentOf(databaseEntity.ToDomain());
            listOfPersons.Should().ContainEquivalentOf(databaseEntity1.ToDomain());
            listOfPersons.Should().ContainEquivalentOf(databaseEntity2.ToDomain());
        }

        [Test]
        public void GetAllClaimantsIfThereAreClaimantAddressesWillReturnThem()
        {
            var databaseEntity = AddPersonRecordToDatabase();

            var address = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity.Id, databaseEntity.HouseId);
            AcademyContext.Addresses.Add(address);
            AcademyContext.SaveChanges();

            var listOfPersons = _classUnderTest.GetAllClaimants();

            listOfPersons
                .First(p => ExtractClaimIdFromAcademyIdString(p.AcademyId).Equals(databaseEntity.Id))
                .AddressList
                .Should().ContainEquivalentOf(address.ToDomain());
        }

        private static int ExtractClaimIdFromAcademyIdString(string academyId)
        {
            return int.Parse(academyId.Split('-').First());
        }

        private Person AddPersonRecordToDatabase()
        {
            var databaseEntity = TestHelper.CreateDatabasePersonEntity();
            AcademyContext.Persons.Add(databaseEntity);
            AcademyContext.SaveChanges();
            return databaseEntity;
        }
    }
}
