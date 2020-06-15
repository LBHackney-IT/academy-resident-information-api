using System;
using System.Configuration;
using System.Threading.Tasks;
using AcademyResidentInformationApi.V1.Boundary.Responses;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AcademyResidentInformationApi.Tests.V1.E2ETests

{
    [TestFixture]
    public class ListClaimantsReturnsAListOfAllClaimants : IntegrationTests<Startup>
    {
        private IFixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        public async Task IfNoQueryParametersListClaimantsReturnsAllClaimantRecordInAcademy()
        {
            var expectedClaimantResponseOne = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext);
            var expectedClaimantResponseTwo = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext);
            var expectedClaimantResponseThree = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext);

            var listUri = new Uri("/api/v1/claimants", UriKind.Relative);

            var response = Client.GetAsync(listUri);
            var statusCode = response.Result.StatusCode;
            statusCode.Should().Be(200);

            var content = response.Result.Content;
            var stringContent = await content.ReadAsStringAsync().ConfigureAwait(true);
            var convertedResponse = JsonConvert.DeserializeObject<ClaimantInformationList>(stringContent);

            convertedResponse.Claimants.Should().ContainEquivalentOf(expectedClaimantResponseOne);
            convertedResponse.Claimants.Should().ContainEquivalentOf(expectedClaimantResponseTwo);
            convertedResponse.Claimants.Should().ContainEquivalentOf(expectedClaimantResponseThree);
        }
    }
}
