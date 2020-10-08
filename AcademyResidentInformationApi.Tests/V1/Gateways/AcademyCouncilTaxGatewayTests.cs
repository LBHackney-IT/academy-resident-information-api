using System.Collections.Generic;
using System.Linq;
using AcademyResidentInformationApi.Tests.V1.Helper;
using AcademyResidentInformationApi.V1.Domain;
using AcademyResidentInformationApi.V1.Gateways;
using AcademyResidentInformationApi.V1.Infrastructure;
using Bogus;
using FluentAssertions;
using NUnit.Framework;
using Address = AcademyResidentInformationApi.V1.Domain.Address;

namespace AcademyResidentInformationApi.Tests.V1.Gateways
{
    [TestFixture]
    public class AcademyCouncilTaxGatewayTests : DatabaseTests
    {
        private AcademyGateway _classUnderTest;
        private readonly Faker _faker = new Faker();


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

        [Test]
        public void GetCouncilTaxPayerInformationByAccountRefReturnsDetailsWithContactInformation()
        {
            var testEmails = new List<string> { "test@email.com" };
            var testPhones = new List<string> { "00000000000", "122223333331" };

            var databaseEntity = SetUpFullTaxPayerDatabaseRecord(accountRef: 123456, emails: testEmails, phoneNumbers: testPhones);

            var response = _classUnderTest.GetTaxPayerById(databaseEntity.AccountRef);

            response.Should().NotBeNull();
            response.AccountRef.Should().Be(123456);
            response.EmailList.Should().BeEquivalentTo(testEmails);
            response.PhoneNumberList.Should().BeEquivalentTo(testPhones);
        }

        [Test]
        public void GetAllTaxPayersIfThereAreNoTaxPayersReturnsAnEmptyList()
        {
            _classUnderTest.GetAllTaxPayers(0, 20).Should().BeEmpty();
        }

        [Test]
        public void GetAllTaxPayersIfThereAreTaxPayersReturnsAListOfAccountNumbersAndNames()
        {
            var databaseEntity1 = SetUpFullTaxPayerDatabaseRecord(1);
            var databaseEntity2 = SetUpFullTaxPayerDatabaseRecord(2);

            var response = _classUnderTest.GetAllTaxPayers(0, 20);

            response.Should().BeOfType<List<TaxPayerInformation>>();
            response.First().Should().BeEquivalentTo(databaseEntity1);
            response.Last().Should().BeEquivalentTo(databaseEntity2);
        }

        [Test]
        public void GetAllTaxPayersReturnsTaxPayersWithCouncilPropertyDetails()
        {
            var databaseEntity = AddTaxPayerDatabaseRecord();
            var property = AddPropertyInformationForTaxPayer(databaseEntity.AccountRef);
            AddContactInformationForTaxPayer(databaseEntity.AccountRef);


            var response = _classUnderTest.GetAllTaxPayers(0, 20);
            response
                .First().TaxPayerAddress.AddressLine1
                .Should().BeEquivalentTo(property.AddressLine1);
        }

        [Test]
        public void GetAllTaxPayersReturnTaxPayersWithContactDetails()
        {

            var testEmails = new List<string> { "test@email1.com", "test@email2.com" };
            var testPhones = new List<string> { "00000000000", "122223333331", "22222222222", null };

            var databaseEntity = SetUpFullTaxPayerDatabaseRecord(emails: testEmails, phoneNumbers: testPhones);

            var response = _classUnderTest.GetAllTaxPayers(0, 20);
            response.First().EmailList.Should().BeEquivalentTo(testEmails);
            response.First().PhoneNumberList.Should().BeEquivalentTo(testPhones);
        }

        [Test]
        public void GetAllTaxPayersWithFirstNameQueryParameterseterReturnsMatchingTaxPayer()
        {
            var databaseEntity1 = SetUpFullTaxPayerDatabaseRecord(firstname: "ciasom");
            var databaseEntity2 = SetUpFullTaxPayerDatabaseRecord(firstname: "shape");
            var databaseEntity3 = SetUpFullTaxPayerDatabaseRecord(firstname: "Ciasom");

            var response = _classUnderTest.GetAllTaxPayers(0, 20, firstname: "ciasom");
            response.Count.Should().Be(2);
            response.Should().ContainEquivalentOf(databaseEntity1);
            response.Should().ContainEquivalentOf(databaseEntity3);
        }

        [Test]
        public void GetAllTaxPayersWildCardSeachWithFirstNameQueryParameterseterReturnsMatchingResident()
        {
            var databaseEntity1 = SetUpFullTaxPayerDatabaseRecord(firstname: "ciasom");
            var databaseEntity2 = SetUpFullTaxPayerDatabaseRecord(firstname: "shape");
            var databaseEntity3 = SetUpFullTaxPayerDatabaseRecord(firstname: "Ciasom");

            var response = _classUnderTest.GetAllTaxPayers(0, 20, firstname: "iaso");
            response.Count.Should().Be(2);
            response.Should().ContainEquivalentOf(databaseEntity1);
            response.Should().ContainEquivalentOf(databaseEntity3);
        }

        [Test]
        public void GetAllTaxPayersWithLastNameQueryParameterseterReturnsMatchingTaxPayer()
        {
            var databaseEntity1 = SetUpFullTaxPayerDatabaseRecord(lastname: "tessellate");
            var databaseEntity2 = SetUpFullTaxPayerDatabaseRecord(lastname: "shape");
            var databaseEntity3 = SetUpFullTaxPayerDatabaseRecord(lastname: "Tessellate");

            var response = _classUnderTest.GetAllTaxPayers(0, 20, lastname: "tessellate");
            response.Count.Should().Be(2);
            response.Should().ContainEquivalentOf(databaseEntity1);
            response.Should().ContainEquivalentOf(databaseEntity3);
        }

        [Test]
        public void GetAllTaxPayersWildCardSearchWithLastNameQueryParameterseterReturnsMatchingTaxPayer()
        {
            var databaseEntity1 = SetUpFullTaxPayerDatabaseRecord(lastname: "tessellate");
            var databaseEntity2 = SetUpFullTaxPayerDatabaseRecord(lastname: "shape");
            var databaseEntity3 = SetUpFullTaxPayerDatabaseRecord(lastname: "Tessellate");

            var response = _classUnderTest.GetAllTaxPayers(0, 20, lastname: "sell");
            response.Count.Should().Be(2);
            response.Should().ContainEquivalentOf(databaseEntity1);
            response.Should().ContainEquivalentOf(databaseEntity3);
        }

        [Test]
        public void GetAllTaxPayersWithFirstAndLastNameQueryParametersetersReturnsMatchingTaxPayerOnlyOnce()
        {
            var databaseEntity = SetUpFullTaxPayerDatabaseRecord(firstname: "ciasom", lastname: "tessellate");

            var response = _classUnderTest.GetAllTaxPayers(0, 20, firstname: "ciasom", lastname: "tessellate");
            response.Count.Should().Be(1);
            response.First().Should().BeEquivalentTo(databaseEntity);
        }

        [Test]
        public void GetAllTaxPayersWithPostcodeQueryParameterseterReturnsMatchingTaxPayer()
        {
            var databaseEntity1 = SetUpFullTaxPayerDatabaseRecord(postcode: "E8 1DY");
            var databaseEntity2 = SetUpFullTaxPayerDatabaseRecord(postcode: "E8 5TG");
            var databaseEntity3 = SetUpFullTaxPayerDatabaseRecord(postcode: "E8 1DY");

            var response = _classUnderTest.GetAllTaxPayers(0, 20, postcode: "E8 1DY");
            response.Count.Should().Be(2);
            response.Should().ContainEquivalentOf(databaseEntity1);
            response.Should().ContainEquivalentOf(databaseEntity3);
        }

        [Test]
        public void GetAllTaxPayersWithNameAndPostcodeQueryParameterseterReturnsMatchingTaxPayer()
        {
            var databaseEntity1 = SetUpFullTaxPayerDatabaseRecord(firstname: "ciasom", postcode: "E8 1DY");
            var databaseEntity2 = SetUpFullTaxPayerDatabaseRecord(firstname: "shape", postcode: "E8 5TG");
            var databaseEntity3 = SetUpFullTaxPayerDatabaseRecord(firstname: "Ciasom", postcode: "E8 5RT");

            var response = _classUnderTest.GetAllTaxPayers(0, 20, firstname: "ciasom", postcode: "E8 1DY");
            response.Count.Should().Be(1);
            response.Should().ContainEquivalentOf(databaseEntity1);
        }

        [TestCase("E81DY")]
        [TestCase("e8 1DY")]
        public void GetAllTaxPayersWithPostCodeQueryParameterseterIgnoresFormatting(string postcodeQuery)
        {
            var databaseEntity = SetUpFullTaxPayerDatabaseRecord(postcode: "E8 1DY");

            var response = _classUnderTest.GetAllTaxPayers(0, 20, postcode: postcodeQuery);
            response.Should().ContainEquivalentOf(databaseEntity);
        }

        [TestCase("1 My Street")]
        [TestCase("My Street")]
        [TestCase("1 My Street, Hackney, London")]
        [TestCase("Hackney")]
        public void GetAllTaxPayersWithAddressQueryParameterseterReturnsMatchingTaxPayer(string addressQuery)
        {
            var databaseEntity1 = SetUpFullTaxPayerDatabaseRecord(address: "1 My Street, Hackney, London");
            var databaseEntity2 = SetUpFullTaxPayerDatabaseRecord(address: "5 Another Street, Lambeth, London");

            var response = _classUnderTest.GetAllTaxPayers(0, 20, address: addressQuery);
            response.Count.Should().Be(1);
        }

        private TaxPayer SetUpFullTaxPayerDatabaseRecord(int? accountRef = null, string firstname = null, string lastname = null,
            string address = null, string postcode = null, List<string> emails = null, List<string> phoneNumbers = null)
        {
            var databaseEntity = AddTaxPayerDatabaseRecord(accountRef, firstname, lastname);
            AddPropertyInformationForTaxPayer(databaseEntity.AccountRef, address, postcode);
            AddContactInformationForTaxPayer(databaseEntity.AccountRef, emails, phoneNumbers);

            return databaseEntity;
        }
        private TaxPayer AddTaxPayerDatabaseRecord(int? accountRef = null, string firstname = null, string lastname = null)
        {
            var databaseEntity = TestHelper.CreateDatabaseTaxPayerEntity(accountRef, firstname, lastname);
            AcademyContext.TaxPayers.Add(databaseEntity);
            AcademyContext.SaveChanges();
            return databaseEntity;
        }

        private CouncilProperty AddPropertyInformationForTaxPayer(int accountRef, string address = null, string postcode = null)
        {
            var occupationDetails = TestHelper.CreateDatabaseOccupationEntityForCouncilProperty(accountRef);
            AcademyContext.Occupations.Add(occupationDetails);
            AcademyContext.SaveChanges();

            var propertyEntity = TestHelper.CreateDatabasePropertyForTaxPayer(occupationDetails.PropertyRef, address, postcode);
            AcademyContext.CouncilProperties.Add(propertyEntity);
            AcademyContext.SaveChanges();
            return propertyEntity;
        }

        private void AddContactInformationForTaxPayer(int accountRef, List<string> emails = null, List<string> phoneNumbers = null)
        {
            if (emails != null)
            {
                foreach (string email in emails)
                {
                    var emailDetails = TestHelper.CreateDatabaseEmailAddressForTaxPayer(accountRef, email);
                    AcademyContext.Emails.Add(emailDetails);
                    AcademyContext.SaveChanges();
                }
            }

            var phoneDetails = TestHelper.CreateDatabasePhoneNumbersForTaxPayer(accountRef, phoneNumbers);
            AcademyContext.PhoneNumbers.Add(phoneDetails);
            AcademyContext.SaveChanges();
        }
    }
}
