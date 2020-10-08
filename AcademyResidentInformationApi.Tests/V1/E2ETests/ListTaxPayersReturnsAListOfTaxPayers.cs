using System;
using System.Linq;
using System.Threading.Tasks;
using AcademyResidentInformationApi.V1.Boundary.Responses;
using AutoFixture;
using Bogus;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AcademyResidentInformationApi.Tests.V1.E2ETests
{
    public class ListTaxPayersReturnsAListOfTaxPayers : IntegrationTests<Startup>
    {
        private IFixture _fixture;
        private readonly Faker _faker = new Faker();

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        public async Task IfNoQueryParametersetersListListTaxPayersReturnsAllTaxPayerRecordsInAcademy()
        {
            var databaseEntity1 = E2ETestHelpers.AddTaxPayerWithRelatesEntitiesToDb(AcademyContext);
            var databaseEntity2 = E2ETestHelpers.AddTaxPayerWithRelatesEntitiesToDb(AcademyContext);
            var databaseEntity3 = E2ETestHelpers.AddTaxPayerWithRelatesEntitiesToDb(AcademyContext);

            var listUri = new Uri("api/v1/tax-payers", UriKind.Relative);

            var response = Client.GetAsync(listUri);
            var statusCode = response.Result.StatusCode;
            statusCode.Should().Be(200);

            var content = response.Result.Content;
            var stringContent = await content.ReadAsStringAsync().ConfigureAwait(true);
            var convertedResponse = JsonConvert.DeserializeObject<TaxPayerInformationList>(stringContent);

            convertedResponse.NextCursor.Should().Be(null);
            convertedResponse.TaxPayers.Should().ContainEquivalentOf(databaseEntity1);
            convertedResponse.TaxPayers.Should().ContainEquivalentOf(databaseEntity2);
            convertedResponse.TaxPayers.Should().ContainEquivalentOf(databaseEntity3);
        }

        [Test]
        public async Task FirstNameLastNameQueryParametersetersReturnsMatchingTaxPayerRecordsFromAcademy()
        {
            var databaseEntity1 = E2ETestHelpers.AddTaxPayerWithRelatesEntitiesToDb(AcademyContext, firstname: "ciasom", lastname: "tessellate");
            var databaseEntity2 = E2ETestHelpers.AddTaxPayerWithRelatesEntitiesToDb(AcademyContext, firstname: "ciasom", lastname: "shape");
            var databaseEntity3 = E2ETestHelpers.AddTaxPayerWithRelatesEntitiesToDb(AcademyContext);

            var queryUri = new Uri("api/v1/tax-payers?first_name=ciasom&last_name=tessellate", UriKind.Relative);

            var response = Client.GetAsync(queryUri);

            var statusCode = response.Result.StatusCode;

            statusCode.Should().Be(200);

            var content = response.Result.Content;
            var stringContent = await content.ReadAsStringAsync().ConfigureAwait(true);
            var convertedResponse = JsonConvert.DeserializeObject<TaxPayerInformationList>(stringContent);

            convertedResponse.TaxPayers.Count.Should().Be(1);
            convertedResponse.TaxPayers.Should().ContainEquivalentOf(databaseEntity1);
        }

        [Test]
        public async Task FirstNameLastNameQueryParametersetersWildcardSearchReturnsMatchingTaxPayerRecordsFromAcademy()
        {
            var databaseEntity1 = E2ETestHelpers.AddTaxPayerWithRelatesEntitiesToDb(AcademyContext, firstname: "ciasom", lastname: "tessellate");
            var databaseEntity2 = E2ETestHelpers.AddTaxPayerWithRelatesEntitiesToDb(AcademyContext, firstname: "ciasom", lastname: "shape");
            var databaseEntity3 = E2ETestHelpers.AddTaxPayerWithRelatesEntitiesToDb(AcademyContext);

            var queryUri = new Uri("api/v1/tax-payers?first_name=iasom&last_name=essellat", UriKind.Relative);

            var response = Client.GetAsync(queryUri);

            var statusCode = response.Result.StatusCode;

            statusCode.Should().Be(200);

            var content = response.Result.Content;
            var stringContent = await content.ReadAsStringAsync().ConfigureAwait(true);
            var convertedResponse = JsonConvert.DeserializeObject<TaxPayerInformationList>(stringContent);

            convertedResponse.TaxPayers.Count.Should().Be(1);
            convertedResponse.TaxPayers.Should().ContainEquivalentOf(databaseEntity1);
        }

        [Test]
        public async Task PostcodeAndAddressQueryParametersetersReturnsMatchingTaxPayerRecordsFromAcademy()
        {
            var databaseEntity1 = E2ETestHelpers.AddTaxPayerWithRelatesEntitiesToDb(AcademyContext, postcode: "E9 1RR", addressLines: "1 Seasame street, Hackney, LDN");
            var databaseEntity2 = E2ETestHelpers.AddTaxPayerWithRelatesEntitiesToDb(AcademyContext, postcode: "E9 1RR", addressLines: "1 Seasame street");
            var nonMatchingDatabaseEntity1 = E2ETestHelpers.AddTaxPayerWithRelatesEntitiesToDb(AcademyContext, postcode: "E4 1RR");
            var nonMatchingDatabaseEntity2 = E2ETestHelpers.AddTaxPayerWithRelatesEntitiesToDb(AcademyContext, addressLines: "1 Seasame street, Hackney, LDN", postcode: "E4 1RR");
            var nonMatchingDatabaseEntity3 = E2ETestHelpers.AddTaxPayerWithRelatesEntitiesToDb(AcademyContext);

            var queryUri = new Uri("api/v1/tax-payers?postcode=e91rr&address=1 Seasame street", UriKind.Relative);

            var response = Client.GetAsync(queryUri);

            var statusCode = response.Result.StatusCode;
            statusCode.Should().Be(200);

            var content = response.Result.Content;
            var stringContent = await content.ReadAsStringAsync().ConfigureAwait(true);
            var convertedResponse = JsonConvert.DeserializeObject<TaxPayerInformationList>(stringContent);

            convertedResponse.TaxPayers.Should().ContainEquivalentOf(databaseEntity1);
            convertedResponse.TaxPayers.Should().ContainEquivalentOf(databaseEntity2);
        }

        [Test]
        public async Task UsingAllQueryParametersetersReturnsMatchingTaxPayerssRecordsFromAcademy()
        {
            var databaseEntity1 = E2ETestHelpers.AddTaxPayerWithRelatesEntitiesToDb(AcademyContext, postcode: "E9 1RR",
                addressLines: "1 Seasame street, Hackney, LDN", firstname: "ciasom", lastname: "shape");
            var nonMatchingDatabaseEntity1 = E2ETestHelpers.AddTaxPayerWithRelatesEntitiesToDb(AcademyContext, postcode: "E4 1RR", addressLines: "1 Seasame street", lastname: "shap");
            var nonMatchingDatabaseEntity2 = E2ETestHelpers.AddTaxPayerWithRelatesEntitiesToDb(AcademyContext, postcode: "E4 1RR", firstname: "ciasom");
            var nonMatchingDatabaseEntity3 = E2ETestHelpers.AddTaxPayerWithRelatesEntitiesToDb(AcademyContext, addressLines: "1 Seasame street, Hackney, LDN", postcode: "E4 1RR");
            var nonMatchingDatabaseEntity4 = E2ETestHelpers.AddTaxPayerWithRelatesEntitiesToDb(AcademyContext);

            var queryUri = new Uri("api/v1/tax-payers?postcode=e91rr&address=1 Seasame street&first_name=ciasom&last_name=shape", UriKind.Relative);
            var response = Client.GetAsync(queryUri);

            var statusCode = response.Result.StatusCode;
            statusCode.Should().Be(200);

            var content = response.Result.Content;
            var stringContent = await content.ReadAsStringAsync().ConfigureAwait(true);
            var convertedResponse = JsonConvert.DeserializeObject<TaxPayerInformationList>(stringContent);

            convertedResponse.TaxPayers.Count.Should().Be(1);
            convertedResponse.TaxPayers.Should().ContainEquivalentOf(databaseEntity1);
        }

        [Test]
        public async Task IfNumberOfTaxPayersMatchingQueryInDbIsLessThanMaxLimitNextCursorShouldBeNull()
        {
            var firstName = _faker.Person.FirstName;
            var lastName = _faker.Person.LastName;
            var addMultipleResidents = Enumerable.Range(0, 10)
                .Select(x =>
                     E2ETestHelpers.AddTaxPayerWithRelatesEntitiesToDb(AcademyContext, firstname: firstName, lastname: lastName, accountRef: x + 1))
                .ToList();
            var listUri = new Uri($"api/v1/tax-payers?first_name={firstName}&last_name={lastName}", UriKind.Relative);
            var response = Client.GetAsync(listUri);
            var statusCode = response.Result.StatusCode;
            statusCode.Should().Be(200);

            var content = response.Result.Content;
            var stringContent = await content.ReadAsStringAsync().ConfigureAwait(true);
            var convertedResponse = JsonConvert.DeserializeObject<TaxPayerInformationList>(stringContent);

            convertedResponse.TaxPayers.Count.Should().Be(addMultipleResidents.Count);
            convertedResponse.NextCursor.Should().Be(null);
        }
        [Test]
        public async Task IfLimitAndCursorIsSuppliedShouldReturnCorrectSetOfTokens()
        {
            var firstName = _faker.Person.FirstName;
            var lastName = _faker.Person.LastName;
            var addMultipleResidents = Enumerable.Range(0, 15)
                .Select(x =>
                     E2ETestHelpers.AddTaxPayerWithRelatesEntitiesToDb(AcademyContext, firstname: firstName, lastname: lastName, accountRef: x + 1))
                .ToList();
            var listUri = new Uri($"api/v1/tax-payers?first_name={firstName}&last_name={lastName}&limit=11&cursor=11", UriKind.Relative);

            var response = Client.GetAsync(listUri);
            var statusCode = response.Result.StatusCode;
            statusCode.Should().Be(200);

            var content = response.Result.Content;
            var stringContent = await content.ReadAsStringAsync().ConfigureAwait(true);
            var convertedResponse = JsonConvert.DeserializeObject<TaxPayerInformationList>(stringContent);

            convertedResponse.TaxPayers.Count.Should().Be(4);
            convertedResponse.NextCursor.Should().Be(null);
        }

        [Test]
        public async Task IfManyTokensInDbAndLimitAndCursorIsSuppliedShouldReturnCorrectSetOfTokens()
        {
            var firstName = _faker.Person.FirstName;
            var lastName = _faker.Person.LastName;
            var addMultipleResidents = Enumerable.Range(0, 35)
                .Select(x =>
                     E2ETestHelpers.AddTaxPayerWithRelatesEntitiesToDb(AcademyContext, firstname: firstName, lastname: lastName, addressLines: "1 Seasame street, Hackney, LDN",
                     accountRef: x + 1))
                .ToList();

            //add a non matching record that should not be part of the next cursor calculation
            var nonMatchingDatabaseRecord = E2ETestHelpers.AddTaxPayerWithRelatesEntitiesToDb(AcademyContext, addressLines: "1 Seasame street, Hackney, LDN", postcode: "E4 1RR");


            var listUri = new Uri($"/api/v1/tax-payers?first_name={firstName}&address=1 Seasame street&limit=11&cursor=11", UriKind.Relative);
            var response = Client.GetAsync(listUri);
            var statusCode = response.Result.StatusCode;
            statusCode.Should().Be(200);

            var content = response.Result.Content;
            var stringContent = await content.ReadAsStringAsync().ConfigureAwait(true);
            var convertedResponse = JsonConvert.DeserializeObject<TaxPayerInformationList>(stringContent);

            convertedResponse.TaxPayers.Count.Should().Be(11);
            convertedResponse.NextCursor.Should().Be("22");
            convertedResponse.TaxPayers.Should().BeEquivalentTo(addMultipleResidents.Skip(11).Take(11));
        }
    }
}
