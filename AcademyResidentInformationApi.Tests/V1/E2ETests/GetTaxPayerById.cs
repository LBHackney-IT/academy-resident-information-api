using System;
using System.Threading.Tasks;
using AcademyResidentInformationApi.V1.Boundary.Responses;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AcademyResidentInformationApi.Tests.V1.E2ETests
{
    public class GetTaxPayerById : IntegrationTests<Startup>
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public async Task GetTaxPayerByIdReturnsTheCorrectInformation()
        {
            var accountRef = _fixture.Create<int>();
            var expectedResponse = E2ETestHelpers.AddTaxPayerWithRelatesEntitiesToDb(AcademyContext, accountRef);

            var requestUri = new Uri($"api/v1/tax-payers/{accountRef}", UriKind.Relative);
            var response = Client.GetAsync(requestUri);
            var statusCode = response.Result.StatusCode;
            statusCode.Should().Be(200);

            var content = response.Result.Content;
            var stringContent = await content.ReadAsStringAsync();
            var convertedResponse = JsonConvert.DeserializeObject<TaxPayerInformationResponse>(stringContent);

            convertedResponse.Should().BeEquivalentTo(expectedResponse);
        }

        [Test]
        public void GetTaxPayerByIdReturns404NotFound()
        {
            var requestUri = new Uri($"api/v1/tax-payers/94837491", UriKind.Relative);
            var response = Client.GetAsync(requestUri);
            var statusCode = response.Result.StatusCode;
            statusCode.Should().Be(404);
        }
    }
}
