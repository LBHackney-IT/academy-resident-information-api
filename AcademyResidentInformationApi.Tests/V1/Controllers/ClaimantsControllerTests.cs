using System.Collections.Generic;
using AcademyResidentInformationApi.V1.Boundary.Requests;
using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Controllers;
using AcademyResidentInformationApi.V1.Domain;
using AcademyResidentInformationApi.V1.UseCase.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ClaimantInformation = AcademyResidentInformationApi.V1.Boundary.Responses.ClaimantInformation;

namespace AcademyResidentInformationApi.Tests.V1.Controllers
{
    [TestFixture]
    public class ClaimantsControllerTests
    {
        private ClaimantsController _classUnderTest;
        private Mock<IGetAllClaimantsUseCase> _mockGetAllClaimantsUseCase;
        private Mock<IGetClaimantByIdUseCase> _mockGetClaimantByIdUseCase;

        [SetUp]
        public void SetUp()
        {
            _mockGetAllClaimantsUseCase = new Mock<IGetAllClaimantsUseCase>();
            _mockGetClaimantByIdUseCase = new Mock<IGetClaimantByIdUseCase>();
            _classUnderTest = new ClaimantsController(_mockGetAllClaimantsUseCase.Object, _mockGetClaimantByIdUseCase.Object);
        }

        [Test]
        public void ListContacts()
        {

            var claimantInfo = new List<ClaimantInformation>()
            {
                new ClaimantInformation()
                {
                    FirstName = "test",
                    LastName = "test",
                    DateOfBirth = "01/01/2020"

                }
            };

            var claimantInformationList = new ClaimantInformationList()
            {
                Claimants = claimantInfo
            };

            var qp = new QueryParameters();

            _mockGetAllClaimantsUseCase.Setup(x => x.Execute(qp, null, 20)).Returns(claimantInformationList);
            var response = _classUnderTest.ListContacts(qp) as OkObjectResult;

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
            response.Value.Should().BeEquivalentTo(claimantInformationList);
        }

        [Test]
        public void ListContactsReturns400IfUseCaseThrowsInvalidCursorException()
        {
            _mockGetAllClaimantsUseCase.Setup(x => x.Execute(It.IsAny<QueryParameters>(), null, 20))
                .Throws(new InvalidCursorException("Wrong cursor"));

            var response = _classUnderTest.ListContacts(new QueryParameters()) as BadRequestObjectResult;

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(400);
            response.Value.Should().Be("Wrong cursor");
        }

        [Test]
        public void ViewRecordTests()
        {
            var singleClaimantInfo = new ClaimantInformation()
            {
                FirstName = "test",
                LastName = "test",
                DateOfBirth = "01/01/2020"
            };

            _mockGetClaimantByIdUseCase.Setup(x => x.Execute(123, 456)).Returns(singleClaimantInfo);
            var response = _classUnderTest.ViewRecord(123, 456) as OkObjectResult;

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
            response.Value.Should().BeEquivalentTo(singleClaimantInfo);
        }

    }
}
