using System;
using AcademyResidentInformationApi.V1.Boundary.Responses;
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
    public class GetTaxPayerByIdUseCaseTest
    {
        private Mock<IAcademyGateway> _mockAcademyGateway;
        private GetTaxPayerByIdUseCase _classUnderTest;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _mockAcademyGateway = new Mock<IAcademyGateway>();
            _classUnderTest = new GetTaxPayerByIdUseCase(_mockAcademyGateway.Object);
        }

        [Test]
        public void ReturnsATaxPayerInformationRecordForTheSpecifiedAccountRef()
        {
            var stubbedTaxPayerInfo = _fixture.Create<TaxPayerInformation>();

            _mockAcademyGateway.Setup(x =>
                    x.GetTaxPayerById(67890))
                .Returns(stubbedTaxPayerInfo);

            var response = _classUnderTest.Execute(67890);
            var expectedResponse = stubbedTaxPayerInfo.ToResponse();

            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(expectedResponse);
        }

        [Test]
        public void IfGatewayReturnsNullThrowNotFoundException()
        {
            TaxPayerInformation nullResult = null;

            _mockAcademyGateway.Setup(x =>
                    x.GetTaxPayerById(It.IsAny<int>()))
                .Returns(nullResult);

            Func<TaxPayerInformationResponse> testDelegate = () => _classUnderTest.Execute(456);
            testDelegate.Should().Throw<TaxPayerNotFoundException>();
        }
    }
}
