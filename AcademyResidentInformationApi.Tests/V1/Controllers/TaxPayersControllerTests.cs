using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using AcademyResidentInformationApi.V1.Domain;
using AcademyResidentInformationApi.V1.UseCase;
using AutoFixture;
using NUnit.Framework;

namespace AcademyResidentInformationApi.Tests.V1.Controllers
{
    [TestFixture]
    public class TaxPayersControllerTests
    {
        private TaxPayersController _classUnderTest;
        private Mock<IGetTaxPayerByIdUseCase> _mockGetTaxPayerByIdUseCase;
        private readonly IFixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _mockGetTaxPayerByIdUseCase = new Mock<IGetTaxPayerByIdUseCase>();
            _classUnderTest = new TaxPayersController(_mockGetTaxPayerByIdUseCase.Object);
        }

        [Test]
        public void ViewTaxPayerReturns200WhenSuccessful()
        {
            var taxPayer = _fixture.Create<TaxPayerInformationResponse>();

            _mockGetTaxPayerByIdUseCase.Setup(x => x.Execute(123)).Returns(taxPayer);
            var response = _classUnderTest.ViewRecord(123) as OkObjectResult;

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
            response.Value.Should().BeEquivalentTo(taxPayer);
        }

        [Test]
        public void ViewReturnsA404IfNotFoundExceptionThrown()
        {
            _mockGetTaxPayerByIdUseCase.Setup(x => x.Execute(It.IsAny<int>())).Throws<TaxPayerNotFoundException>();
            var response = _classUnderTest.ViewRecord(123) as NotFoundResult;
            response.StatusCode.Should().Be(404);
        }
    }
}
