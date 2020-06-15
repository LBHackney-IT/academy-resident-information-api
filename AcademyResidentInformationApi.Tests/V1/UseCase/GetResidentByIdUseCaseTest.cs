using AcademyResidentInformationApi.V1.Domain;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Gateways;
using AcademyResidentInformationApi.V1.UseCase;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace AcademyResidentInformationApi.Tests.V1.UseCase
{
    [TestFixture]
    public class GetResidentByIdUseCaseTest
    {
        private Mock<IAcademyGateway> _mockAcademyGateway;
        private GetResidentByIdUseCase _classUnderTest;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _mockAcademyGateway = new Mock<IAcademyGateway>();
            _classUnderTest = new GetResidentByIdUseCase(_mockAcademyGateway.Object);
        }

        [Test]
        public void ReturnsAResidentInformationRecordForTheSpecifiedID()
        {
            var stubbedResidentInfo = _fixture.Create<ResidentInformation>();
            var testAcademyId = "1234-5678";

            _mockAcademyGateway.Setup(x =>
                    x.GetResidentById(1234, 5678))
                .Returns(stubbedResidentInfo);

            var response = _classUnderTest.Execute(testAcademyId);
            var expectedResponse = stubbedResidentInfo.ToResponse();

            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
