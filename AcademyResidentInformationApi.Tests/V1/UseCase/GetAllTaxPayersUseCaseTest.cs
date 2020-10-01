using System;
using System.Linq;
using AcademyResidentInformationApi.V1.Boundary.Requests;
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
    public class GetAllTaxPayersUseCaseTest
    {
        private Mock<IAcademyGateway> _mockAcademyGateway;
        private GetAllTaxPayersUseCase _classUnderTest;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _mockAcademyGateway = new Mock<IAcademyGateway>();
            _classUnderTest = new GetAllTaxPayersUseCase(_mockAcademyGateway.Object);
        }

        [Test]
        public void ReturnsTaxPayerInformationList()
        {
            var stubbedTaxPayers = _fixture.CreateMany<TaxPayerInformation>().ToList();

            _mockAcademyGateway.Setup(x =>
                x.GetAllTaxPayers("ciasom", "tessellate", "E8 1DY", "1 Montage street"))
                .Returns(stubbedTaxPayers);

            var qp = new QueryParameters
            {
                FirstName = "ciasom",
                LastName = "tessellate",
                Postcode = "E8 1DY",
                Address = "1 Montage street"
            };
            var response = _classUnderTest.Execute(qp);
            response.Should().NotBeNull();
            response.TaxPayers.Should().BeEquivalentTo(stubbedTaxPayers.ToResponse());
        }
    }
}
