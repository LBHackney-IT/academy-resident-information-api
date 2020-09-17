using System;
using AcademyResidentInformationApi.V1.Domain;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Gateways;
using AcademyResidentInformationApi.V1.UseCase;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using ClaimantInformationResponse = AcademyResidentInformationApi.V1.Boundary.Responses.ClaimantInformation;

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

            _mockAcademyGateway.Setup(x =>
                    x.GetClaimantById(1234, 5678))
                .Returns(stubbedClaimantInfo);

            var response = _classUnderTest.Execute(1234, 5678);
            var expectedResponse = stubbedClaimantInfo.ToResponse();

            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(expectedResponse);
        }

        [Test]
        public void IfGatewayReturnsNullThrowNotFoundException()
        {
            ClaimantInformation nullResult = null;

            _mockAcademyGateway.Setup(x =>
                    x.GetClaimantById(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(nullResult);

            Func<ClaimantInformationResponse> testDelegate = () => _classUnderTest.Execute(123, 456);
            testDelegate.Should().Throw<ClaimantNotFoundException>();
        }
    }
}
