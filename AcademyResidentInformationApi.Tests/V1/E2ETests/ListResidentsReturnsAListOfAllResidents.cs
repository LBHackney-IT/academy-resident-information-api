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
    public class ListResidentsReturnsAListOfAllResidents : IntegrationTests<Startup>
    {
        private IFixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Ignore("Incomplete end-to-end test")]
        public async Task IfNoQueryParametersListResidentsReturnsAllResidentRecordInAcademy()
        {
            var expectedResidentResponseOne = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext);
            var expectedResidentResponseTwo = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext);
            var expectedResidentResponseThree = E2ETestHelpers.AddPersonWithRelatesEntitiesToDb(AcademyContext);

            var listUri = new Uri("api/v1/residents");

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
    }
}
