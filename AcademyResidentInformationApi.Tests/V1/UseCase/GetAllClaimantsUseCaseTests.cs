using System.Linq;
using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Gateways;
using AcademyResidentInformationApi.V1.UseCase;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using ClaimantInformation = AcademyResidentInformationApi.V1.Domain.ClaimantInformation;

namespace AcademyResidentInformationApi.Tests.V1.UseCase
{
    [TestFixture]
    public class GetAllClaimantsUseCaseTests
    {
        private Mock<IAcademyGateway> _mockAcademyGateway;
        private GetAllClaimantsUseCase _classUnderTest;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _mockAcademyGateway = new Mock<IAcademyGateway>();
            _classUnderTest = new GetAllClaimantsUseCase(_mockAcademyGateway.Object);
        }

        [Test]
        public void ReturnsClaimantInformationList()
        {
            var stubbedClaimants = _fixture.CreateMany<ClaimantInformation>();

            _mockAcademyGateway.Setup(x => x.GetAllClaimants(null, null))
                .Returns(stubbedClaimants.ToList());

            var response = _classUnderTest.Execute();

            response.Should().NotBeNull();
            response.Claimants.Should().BeEquivalentTo(stubbedClaimants.ToResponse());
        }
    }
}
