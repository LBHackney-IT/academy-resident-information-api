using System.Linq;
using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Gateways;
using AcademyResidentInformationApi.V1.UseCase;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using ResidentInformation = AcademyResidentInformationApi.V1.Domain.ResidentInformation;

namespace AcademyResidentInformationApi.Tests.V1.UseCase
{
    [TestFixture]
    public class GetAllResidentsUseCaseTests
    {
        private Mock<IAcademyGateway> _mockAcademyGateway;
        private GetAllResidentsUseCase _classUnderTest;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _mockAcademyGateway = new Mock<IAcademyGateway>();
            _classUnderTest = new GetAllResidentsUseCase(_mockAcademyGateway.Object);
        }

        [Test]
        public void ReturnsResidentInformationList()
        {
            var stubbedResidents = _fixture.CreateMany<ResidentInformation>();

            _mockAcademyGateway.Setup(x => x.GetAllResidents(null, null))
                .Returns(stubbedResidents.ToList());

            var response = _classUnderTest.Execute();

            response.Should().NotBeNull();
            response.Residents.Should().BeEquivalentTo(stubbedResidents.ToResponse());
        }
    }
}
