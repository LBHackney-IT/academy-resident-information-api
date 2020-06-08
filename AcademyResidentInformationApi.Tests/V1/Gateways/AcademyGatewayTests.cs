using System.Linq;
using AcademyResidentInformationApi.Tests.V1.Helper;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Gateways;
using AcademyResidentInformationApi.V1.Infrastructure;
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
            _classUnderTest.GetAllResidents(0, 20).Should().BeEmpty();
        }

        [Test]
        public void GetAllResidentIfThereAreResidentsWillReturnThem()
        {
            var databaseEntity = AddPersonRecordToDatabase();

            var listOfPersons = _classUnderTest.GetAllResidents(0, 20);

            listOfPersons.Should().BeNullOrEmpty();
        }

        [Test]
        public void GetAllResidentsIfThereAreResidentAddressesWillReturnThem()
        {
            var databaseEntity = AddPersonRecordToDatabase();

            var address = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity.Id);
            AcademyContext.Addresses.Add(address);
            AcademyContext.SaveChanges();

            var listOfPersons = _classUnderTest.GetAllResidents(0, 20, postcode: address.PostCode);

            listOfPersons
                .First(p => p.AcademyId.Equals(databaseEntity.Id.ToString()))
                .ResidentAddress
                .Should().BeEquivalentTo(address.ToDomain());
        }

        [Test]
        public void GetAllResidentsWithFirstNameQueryParameterReturnsMatchingResident()
        {
            var databaseEntity = AddPersonRecordToDatabase(firstname: "ciasom");
            var databaseEntity1 = AddPersonRecordToDatabase(firstname: "shape");
            var databaseEntity2 = AddPersonRecordToDatabase(firstname: "Ciasom");

            var address = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity.Id);
            AcademyContext.Addresses.Add(address);
            AcademyContext.SaveChanges();

            var address1 = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity1.Id);
            AcademyContext.Addresses.Add(address1);
            AcademyContext.SaveChanges();

            var address2 = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity2.Id);
            AcademyContext.Addresses.Add(address2);
            AcademyContext.SaveChanges();

            var domainEntity = databaseEntity.ToDomain();
            domainEntity.ResidentAddress = address.ToDomain();

            var domainEntity2 = databaseEntity2.ToDomain();
            domainEntity2.ResidentAddress = address2.ToDomain();

            var listOfPersons = _classUnderTest.GetAllResidents(cursor: 0, limit: 20, firstname: "ciasom");
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

            var address = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity.Id);
            AcademyContext.Addresses.Add(address);
            AcademyContext.SaveChanges();

            var address1 = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity1.Id);
            AcademyContext.Addresses.Add(address1);
            AcademyContext.SaveChanges();

            var address2 = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity2.Id);
            AcademyContext.Addresses.Add(address2);
            AcademyContext.SaveChanges();

            var domainEntity = databaseEntity.ToDomain();
            domainEntity.ResidentAddress = address.ToDomain();

            var domainEntity2 = databaseEntity2.ToDomain();
            domainEntity2.ResidentAddress = address2.ToDomain();

            var listOfPersons = _classUnderTest.GetAllResidents(cursor: 0, limit: 20, lastname: "tessellate");
            listOfPersons.Count.Should().Be(2);
            listOfPersons.Should().ContainEquivalentOf(domainEntity);
            listOfPersons.Should().ContainEquivalentOf(domainEntity2);
        }

        [Test]
        public void GetAllResidentsWithNameQueryParametersReturnsMatchingResidentOnlyOnce()
        {
            var databaseEntity = AddPersonRecordToDatabase(firstname: "ciasom", lastname: "Tessellate");

            var address = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity.Id);
            AcademyContext.Addresses.Add(address);
            AcademyContext.SaveChanges();

            var listOfPersons = _classUnderTest.GetAllResidents(cursor: 0, limit: 20, firstname: "ciasom", lastname: "Tessellate");
            listOfPersons.Count.Should().Be(1);
            listOfPersons.First().AcademyId.Should().Be(databaseEntity.Id.ToString());
        }

        [Test]
        public void GetAllResidentsWithPostCodeQueryParameterReturnsMatchingResident()
        {
            var databaseEntity = AddPersonRecordToDatabase();
            var databaseEntity1 = AddPersonRecordToDatabase();

            var address = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity.Id, "E8 1DY");
            AcademyContext.Addresses.Add(address);
            AcademyContext.SaveChanges();

            var address1 = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity1.Id, "E8 5TG");
            AcademyContext.Addresses.Add(address1);
            AcademyContext.SaveChanges();

            var listOfPersons = _classUnderTest.GetAllResidents(cursor: 0, limit: 20, postcode: "E8 1DY");
            listOfPersons.Count.Should().Be(1);
            listOfPersons
                .First(p => p.AcademyId.Equals(databaseEntity.Id.ToString()))
                .ResidentAddress
                .Should().BeEquivalentTo(address.ToDomain());
        }

        [Test]
        public void GetAllResidentsWithNameAndPostCodeQueryParameterReturnsMatchingResident()
        {
            var databaseEntity = AddPersonRecordToDatabase(firstname: "ciasom");
            var address = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity.Id, "E8 1DY");
            AcademyContext.Addresses.Add(address);
            AcademyContext.SaveChanges();

            var databaseEntity1 = AddPersonRecordToDatabase(firstname: "wrong name");
            var address1 = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity1.Id, "E8 1DY");
            AcademyContext.Addresses.Add(address1);
            AcademyContext.SaveChanges();

            var databaseEntity2 = AddPersonRecordToDatabase(firstname: "ciasom");
            var address2 = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity2.Id, "E8 5RT");
            AcademyContext.Addresses.Add(address2);
            AcademyContext.SaveChanges();

            var listOfPersons = _classUnderTest.GetAllResidents(cursor: 0, limit: 20, firstname: "ciasom", postcode: "E8 1DY").ToList();

            listOfPersons.Count.Should().Be(1);
            listOfPersons.First().AcademyId.Should().Be(databaseEntity.Id.ToString());
            listOfPersons.First()
                .ResidentAddress
                .Should().BeEquivalentTo(address.ToDomain());
        }

        [TestCase("E81DY")]
        [TestCase("e8 1DY")]
        public void GetAllResidentsWithPostCodeQueryParameterIgnoresFormatting(string postcode)
        {
            var databaseEntity = AddPersonRecordToDatabase();

            var address = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity.Id, postcode);
            AcademyContext.Addresses.Add(address);
            AcademyContext.SaveChanges();

            var listOfPersons = _classUnderTest.GetAllResidents(cursor: 0, limit: 20, postcode: "E8 1DY");

            listOfPersons.Count.Should().Be(1);

            listOfPersons.First().AcademyId.Should().Be(databaseEntity.Id.ToString());
            listOfPersons.First().ResidentAddress
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

            var address = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity.Id, address: "1 My Street, Hackney, London");
            AcademyContext.Addresses.Add(address);
            AcademyContext.SaveChanges();

            var address1 = TestHelper.CreateDatabaseAddressForPersonId(databaseEntity1.Id, address: "5 Another Street, Lambeth, London");
            AcademyContext.Addresses.Add(address1);
            AcademyContext.SaveChanges();

            var listOfPersons = _classUnderTest.GetAllResidents(cursor: 0, limit: 20, address: addressQuery).ToList();
            listOfPersons.Count.Should().Be(1);
            listOfPersons
                .First(p => p.AcademyId.Equals(databaseEntity.Id.ToString()))
                .ResidentAddress
                .Should().BeEquivalentTo(address.ToDomain());
        }
        private Person AddPersonRecordToDatabase(string firstname = null, string lastname = null, int? id = null)
        {
            var databaseEntity = TestHelper.CreateDatabasePersonEntity(firstname, lastname, id);
            AcademyContext.Persons.Add(databaseEntity);
            AcademyContext.SaveChanges();
            return databaseEntity;
        }

    }
}
