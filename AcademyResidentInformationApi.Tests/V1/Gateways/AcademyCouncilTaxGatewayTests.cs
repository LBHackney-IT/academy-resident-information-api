using AcademyResidentInformationApi.Tests.V1.Helper;
using AcademyResidentInformationApi.V1.Domain;
using AcademyResidentInformationApi.V1.Gateways;
using AcademyResidentInformationApi.V1.Infrastructure;
using FluentAssertions;
using NUnit.Framework;
using Address = AcademyResidentInformationApi.V1.Domain.Address;

namespace AcademyResidentInformationApi.Tests.V1.Gateways
{
    [TestFixture]
    public class AcademyCouncilTaxGatewayTests : DatabaseTests
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
        public void GetCouncilTaxPayerInformationByAccountRefWhenThereAreNoMatchingRecordsReturnsNull()
        {
            var response = _classUnderTest.GetTaxPayerById(123456);
            response.Should().BeNull();
        }

        [Test]
        public void GetCouncilTaxPayerInformationByAccountRefReturnsPersonalDetailsFromTheCTAccountTable()
        {
            var databaseEntity = AddTaxPayerDatabaseRecord(123456);
            AddPropertyInformationForTaxPayer(databaseEntity.AccountRef);

            var response = _classUnderTest.GetTaxPayerById(databaseEntity.AccountRef);

            response.Should().NotBeNull();
            response.AccountRef.Should().Be(123456);
            response.FirstName.Should().Be(databaseEntity.FirstName);
            response.LastName.Should().Be(databaseEntity.LastName);
        }

        [Test]
        public void GetCouncilTaxPayerInformationByAccountRefReturnsDetailsFromTheCTPropertyTable()
        {
            var databaseEntity = AddTaxPayerDatabaseRecord(123456);
            var propertyEntity = AddPropertyInformationForTaxPayer(databaseEntity.AccountRef);

            var expectedTaxPayerAddress = new Address
            {
                AddressLine1 = propertyEntity.AddressLine1,
                AddressLine2 = propertyEntity.AddressLine2,
                AddressLine3 = propertyEntity.AddressLine3,
                AddressLine4 = propertyEntity.AddressLine4,
                PostCode = propertyEntity.PostCode
            };

            var response = _classUnderTest.GetTaxPayerById(databaseEntity.AccountRef);

            response.Should().NotBeNull();
            response.AccountRef.Should().Be(123456);
            response.Uprn.Should().Be(propertyEntity.Uprn);
            response.TaxPayerAddress.Should().BeEquivalentTo(expectedTaxPayerAddress);
        }

        private TaxPayer AddTaxPayerDatabaseRecord(int? accountRef, string firstname = null, string lastname = null)
        {
            var databaseEntity = TestHelper.CreateDatabaseTaxPayerEntity(accountRef, firstname, lastname);
            AcademyContext.TaxPayers.Add(databaseEntity);
            AcademyContext.SaveChanges();
            return databaseEntity;
        }

        private CouncilProperty AddPropertyInformationForTaxPayer(int accountRef)
        {
            var occupationDetails = TestHelper.CreateDatabaseOccupationEntityForCouncilProperty(accountRef);
            AcademyContext.Occupations.Add(occupationDetails);
            AcademyContext.SaveChanges();

            var propertyEntity = TestHelper.CreateDatabasePropertyForTaxPayer(occupationDetails.PropertyRef);
            AcademyContext.CouncilProperties.Add(propertyEntity);
            AcademyContext.SaveChanges();
            return propertyEntity;
        }
    }
}
