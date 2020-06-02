using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Gateways;
using AcademyResidentInformationApi.V1.UseCase;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace MosaicResidentInformationApi.Tests.V1.UseCase
{
    [TestFixture]
    public class GetEntityByIdCaseTest
    {
        private Mock<IAcademyGateway> _mockAcademyGateway;
        private GetResidentByIdUseCase _classUnderTest;
        private Fixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _mockAcademyGateway = new Mock<IAcademyGateway>();
            _classUnderTest = new GetResidentByIdUseCase(_mockAcademyGateway.Object);
        }

        [Test]
        [Ignore("Response object needed")]
        public void ReturnsResidentInformationList()
        {
            var stubbedResidentInfo = _fixture.Create<ResidentInformation>();

            _mockAcademyGateway.Setup(x =>
                    x.GetResidentById(12345))
                .Returns(stubbedResidentInfo);

            var response = _classUnderTest.Execute(12345);
            //var expectedResponse = stubbedResidentInfo.ToResponse();

            response.Should().NotBeNull();
            //response.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
