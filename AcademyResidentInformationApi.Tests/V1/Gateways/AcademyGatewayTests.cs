using System;
using System.Globalization;
using System.Linq;
using AcademyResidentInformationApi.Tests.V1.Helper;
using AcademyResidentInformationApi.V1.Domain;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Gateways;
using AcademyResidentInformationApi.V1.Infrastructure;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;

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
        public void GetAllResidentsIfThereAreNoResidentsReturnsAnEmptyList()
        {
            _classUnderTest.GetAllResidents().Should().BeEmpty();
        }

        [Test]
        public void GetAllResidentIfThereAreResidentsWillReturnThem()
        {
            var databaseEntity = AddPersonRecordToDatabase();
            var databaseEntity1 = AddPersonRecordToDatabase();
            var databaseEntity2 = AddPersonRecordToDatabase();

            var listOfPersons = _classUnderTest.GetAllResidents();

            listOfPersons.Should().ContainEquivalentOf(databaseEntity.ToDomain());
            listOfPersons.Should().ContainEquivalentOf(databaseEntity1.ToDomain());
            listOfPersons.Should().ContainEquivalentOf(databaseEntity2.ToDomain());
        }

        [Test]
        public void GetAllResidentsIfThereAreResidentAddressesWillReturnThem()
        {
            var databaseEntity = AddPersonRecordToDatabase();

            var address = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity.Id);
            AcademyContext.Addresses.Add(address);
            AcademyContext.SaveChanges();

            var listOfPersons = _classUnderTest.GetAllResidents();

            listOfPersons
                .First(p => p.AcademyId.Equals(databaseEntity.Id.ToString(CultureInfo.InvariantCulture), StringComparison.InvariantCulture))
                .AddressList
                .Should().ContainEquivalentOf(address.ToDomain());
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
