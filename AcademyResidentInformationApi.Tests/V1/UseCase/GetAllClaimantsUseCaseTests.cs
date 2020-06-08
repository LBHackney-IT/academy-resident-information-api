using System.Linq;
using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Gateways;
using AcademyResidentInformationApi.V1.UseCase;
using AutoFixture;
using FluentAssertions;
using Moq;
using AcademyResidentInformationApi.V1.Boundary.Requests;
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
            var expectedResponse = new ClaimantInformationList()
            {
                Claimants = stubbedClaimants.ToResponse()
            };

            _mockAcademyGateway.Setup(x =>
                x.GetAllClaimants(0, 20, "ciasom", "tessellate", "E8 1DY", "1 Montage street"))
                .Returns(stubbedClaimants.ToList());

            var cqp = new ClaimantQueryParam()
            {
                FirstName = "ciasom",
                LastName = "tessellate",
                Postcode = "E8 1DY",
                Address = "1 Montage street"
            };
            var response = _classUnderTest.Execute(cqp, 0, 20);

            response.Should().NotBeNull();
            response.Claimants.Should().BeEquivalentTo(stubbedClaimants.ToResponse());
        }
    }
}
