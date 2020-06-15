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
    public class GetClaimantByIdUseCaseTest
    {
        private Mock<IAcademyGateway> _mockAcademyGateway;
        private GetClaimantByIdUseCase _classUnderTest;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _mockAcademyGateway = new Mock<IAcademyGateway>();
            _classUnderTest = new GetClaimantByIdUseCase(_mockAcademyGateway.Object);
        }

        [Test]
        public void ReturnsAClaimantInformationRecordForTheSpecifiedID()
        {
            var stubbedClaimantInfo = _fixture.Create<ClaimantInformation>();
            var testAcademyId = "1234-5678";

            _mockAcademyGateway.Setup(x =>
                    x.GetClaimantById(1234, 5678))
                .Returns(stubbedClaimantInfo);

            var response = _classUnderTest.Execute(testAcademyId);
            var expectedResponse = stubbedClaimantInfo.ToResponse();

            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
