using AcademyResidentInformationApi.Tests.V1.Helper;
using AcademyResidentInformationApi.V1.Gateways;
using AcademyResidentInformationApi.V1.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

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
            var response = _classUnderTest.GetTaxPayerById(databaseEntity.AccountRef);

            response.Should().NotBeNull();
            response.AccountRef.Should().Be(123456);
            response.FirstName.Should().Be(databaseEntity.FirstName);
            response.LastName.Should().Be(databaseEntity.LastName);
        }

        private TaxPayer AddTaxPayerDatabaseRecord(int? accountRef, string firstname = null, string lastname = null)
        {
            var databaseEntity = TestHelper.CreateDatabaseTaxPayerEntity(accountRef, firstname, lastname);
            AcademyContext.TaxPayers.Add(databaseEntity);
            AcademyContext.SaveChanges();
            return databaseEntity;
        }
    }
}
