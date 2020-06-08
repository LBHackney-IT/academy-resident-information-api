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
    public class ListResidentsReturnsAListOfAllResidents : IntegrationTests<Startup>
    {
        private IFixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        public async Task IfNoQueryParametersListResidentsReturnsAllResidentRecordInAcademy()
        {
            var expectedResidentResponseOne = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext);
            var expectedResidentResponseTwo = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext);
            var expectedResidentResponseThree = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext);

            var listUri = new Uri("api/v1/residents", UriKind.Relative);

            var response = Client.GetAsync(listUri);
            var statusCode = response.Result.StatusCode;
            statusCode.Should().Be(200);

            var content = response.Result.Content;
            var stringContent = await content.ReadAsStringAsync().ConfigureAwait(true);
            var convertedResponse = JsonConvert.DeserializeObject<ResidentInformationList>(stringContent);

            convertedResponse.Residents.Should().ContainEquivalentOf(expectedResidentResponseOne);
            convertedResponse.Residents.Should().ContainEquivalentOf(expectedResidentResponseTwo);
            convertedResponse.Residents.Should().ContainEquivalentOf(expectedResidentResponseThree);
        }

        [Test]
        public async Task FirstNameLastNameQueryParametersReturnsMatchingResidentRecordsFromAcademy()
        {
            var expectedResidentResponseOne = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext, firstname: "ciasom", lastname: "tessellate");
            var expectedResidentResponseTwo = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext, firstname: "ciasom", lastname: "shape");
            var expectedResidentResponseThree = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext);

            var queryUri = new Uri("api/v1/residents?first_name=ciasom&last_name=tessellate", UriKind.Relative);

            var response = Client.GetAsync(queryUri);

            var statusCode = response.Result.StatusCode;
            statusCode.Should().Be(200);

            var content = response.Result.Content;
            var stringContent = await content.ReadAsStringAsync().ConfigureAwait(true);
            var convertedResponse = JsonConvert.DeserializeObject<ResidentInformationList>(stringContent);

            convertedResponse.Residents.Count.Should().Be(1);
            convertedResponse.Residents.Should().ContainEquivalentOf(expectedResidentResponseOne);
        }

        [Test]
        public async Task PostcodeAndAddressQueryParametersReturnsMatchingResidentsRecordsFromAcademy()
        {
            var matchingResidentOne = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext, postcode: "ER 1RR", addressLines: "1 Seasame street, Hackney, LDN");
            var matchingResidentTwo = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext, postcode: "ER 1RR", addressLines: "1 Seasame street");
            var nonMatchingResident1 = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext, postcode: "E4 1RR");
            var nonMatchingResident2 = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext, addressLines: "1 Seasame street, Hackney, LDN");
            var nonMatchingResident3 = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext);

            var queryUri = new Uri("api/v1/residents?postcode=er1rr&address=1 Seasame street", UriKind.Relative);

            var response = Client.GetAsync(queryUri);

            var statusCode = response.Result.StatusCode;
            statusCode.Should().Be(200);

            var content = response.Result.Content;
            var stringContent = await content.ReadAsStringAsync().ConfigureAwait(true);
            var convertedResponse = JsonConvert.DeserializeObject<ResidentInformationList>(stringContent);

            convertedResponse.Residents.Count.Should().Be(2);
            convertedResponse.Residents.Should().ContainEquivalentOf(matchingResidentOne);
            convertedResponse.Residents.Should().ContainEquivalentOf(matchingResidentTwo);
        }

        [Test]
        public async Task UsingAllQueryParametersReturnsMatchingResidentsRecordsFromAcademy()
        {
            var matchingResidentOne = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext, postcode: "ER 1RR",
                addressLines: "1 Seasame street, Hackney, LDN", firstname: "ciasom", lastname: "shape");
            var nonmatchingResidentTwo = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext, postcode: "ER 1RR", addressLines: "1 Seasame street", lastname: "shap");
            var nonMatchingResident1 = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext, postcode: "E4 1RR", firstname: "ciasom");
            var nonMatchingResident2 = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext, addressLines: "1 Seasame street, Hackney, LDN");
            var nonMatchingResident3 = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext);

            var queryUri = new Uri("api/v1/residents?postcode=er1rr&address=1 Seasame street&first_name=ciasom&last_name=shape", UriKind.Relative);
            var response = Client.GetAsync(queryUri);

            var statusCode = response.Result.StatusCode;
            statusCode.Should().Be(200);

            var content = response.Result.Content;
            var stringContent = await content.ReadAsStringAsync().ConfigureAwait(true);
            var convertedResponse = JsonConvert.DeserializeObject<ResidentInformationList>(stringContent);

            convertedResponse.Residents.Count.Should().Be(1);
            convertedResponse.Residents.Should().ContainEquivalentOf(matchingResidentOne);
        }
    }
}
