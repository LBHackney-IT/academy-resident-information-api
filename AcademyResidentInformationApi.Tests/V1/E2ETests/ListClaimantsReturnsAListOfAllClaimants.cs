using System;
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
        [Test]
        public async Task FirstNameLastNameQueryParametersReturnsMatchingClaimantRecordsFromAcademy()
        {
            var expectedClaimantResponseOne = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext, firstname: "ciasom", lastname: "tessellate");
            var expectedClaimantResponseTwo = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext, firstname: "ciasom", lastname: "shape");
            var expectedClaimantResponseThree = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext);

            var queryUri = new Uri("api/v1/claimants?first_name=ciasom&last_name=tessellate", UriKind.Relative);

            var response = Client.GetAsync(queryUri);

            var statusCode = response.Result.StatusCode;

            statusCode.Should().Be(200);

            var content = response.Result.Content;
            var stringContent = await content.ReadAsStringAsync().ConfigureAwait(true);
            var convertedResponse = JsonConvert.DeserializeObject<ClaimantInformationList>(stringContent);

            convertedResponse.Claimants.Count.Should().Be(1);
            convertedResponse.Claimants.Should().ContainEquivalentOf(expectedClaimantResponseOne);
        }
        [Test]
        public async Task FirstNameLastNameQueryParametersWildcardSearchReturnsMatchingClaimantRecordsFromAcademy()
        {
            var expectedClaimantResponseOne = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext, firstname: "ciasom", lastname: "tessellate");
            var expectedClaimantResponseTwo = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext, firstname: "ciasom", lastname: "shape");
            var expectedClaimantResponseThree = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext);

            var queryUri = new Uri("api/v1/claimants?first_name=iasom&last_name=essellat", UriKind.Relative);

            var response = Client.GetAsync(queryUri);

            var statusCode = response.Result.StatusCode;

            statusCode.Should().Be(200);

            var content = response.Result.Content;
            var stringContent = await content.ReadAsStringAsync().ConfigureAwait(true);
            var convertedResponse = JsonConvert.DeserializeObject<ClaimantInformationList>(stringContent);

            convertedResponse.Claimants.Count.Should().Be(1);
            convertedResponse.Claimants.Should().ContainEquivalentOf(expectedClaimantResponseOne);
        }

        [Test]
        public async Task PostcodeAndAddressQueryParametersReturnsMatchingClaimantsRecordsFromAcademy()
        {
            var matchingClaimantOne = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext, postcode: "E1 1RR", addressLines: "1 Seasame street, Hackney, LDN");
            var matchingClaimantTwo = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext, postcode: "E1 1RR", addressLines: "1 Seasame street");
            var nonMatchingClaimant1 = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext, postcode: "E4 1RR");
            var nonMatchingClaimant2 = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext, addressLines: "1 Seasame street, Hackney, LDN", postcode: "E4 1RR");
            var nonMatchingClaimant3 = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext);

            var queryUri = new Uri("api/v1/claimants?postcode=e11rr&address=1 Seasame street", UriKind.Relative);

            var response = Client.GetAsync(queryUri);

            var statusCode = response.Result.StatusCode;
            statusCode.Should().Be(200);

            var content = response.Result.Content;
            var stringContent = await content.ReadAsStringAsync().ConfigureAwait(true);
            var convertedResponse = JsonConvert.DeserializeObject<ClaimantInformationList>(stringContent);

            convertedResponse.Claimants.Should().ContainEquivalentOf(matchingClaimantOne);
            convertedResponse.Claimants.Should().ContainEquivalentOf(matchingClaimantTwo);
        }

        [Test]
        public async Task UsingAllQueryParametersReturnsMatchingClaimantsRecordsFromAcademy()
        {
            var matchingClaimantOne = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext, postcode: "E9 1RR",
                addressLines: "1 Seasame street, Hackney, LDN", firstname: "ciasom", lastname: "shape");
            var nonMatchingClaimantTwo = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext, postcode: "E4 1RR", addressLines: "1 Seasame street", lastname: "shap");
            var nonMatchingClaimant1 = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext, postcode: "E4 1RR", firstname: "ciasom");
            var nonMatchingClaimant2 = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext, addressLines: "1 Seasame street, Hackney, LDN", postcode: "E4 1RR");
            var nonMatchingClaimant3 = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext);

            var queryUri = new Uri("api/v1/claimants?postcode=e91rr&address=1 Seasame street&first_name=ciasom&last_name=shape", UriKind.Relative);
            var response = Client.GetAsync(queryUri);

            var statusCode = response.Result.StatusCode;
            statusCode.Should().Be(200);

            var content = response.Result.Content;
            var stringContent = await content.ReadAsStringAsync().ConfigureAwait(true);
            var convertedResponse = JsonConvert.DeserializeObject<ClaimantInformationList>(stringContent);

            convertedResponse.Claimants.Count.Should().Be(1);
            convertedResponse.Claimants.Should().ContainEquivalentOf(matchingClaimantOne);
        }
    }
}
