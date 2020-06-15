using System.Collections.Generic;
using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Controllers;
using AcademyResidentInformationApi.V1.UseCase.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace AcademyResidentInformationApi.Tests.V1.Controllers
{
    [TestFixture]
    public class AcademyControllerTests
    {
        private AcademyController _classUnderTest;
        private Mock<IGetAllClaimantsUseCase> _mockGetAllClaimantsUseCase;
        private Mock<IGetClaimantByIdUseCase> _mockGetClaimantByIdUseCase;

        [SetUp]
        public void SetUp()
        {
            _mockGetAllClaimantsUseCase = new Mock<IGetAllClaimantsUseCase>();
            _mockGetClaimantByIdUseCase = new Mock<IGetClaimantByIdUseCase>();
            _classUnderTest = new AcademyController(_mockGetAllClaimantsUseCase.Object, _mockGetClaimantByIdUseCase.Object);
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

            _mockGetAllClaimantsUseCase.Setup(x => x.Execute()).Returns(claimantInformationList);
            var response = _classUnderTest.ListContacts() as OkObjectResult;

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
            response.Value.Should().BeEquivalentTo(claimantInformationList);
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

            var testAcademyId = "5678-1234";

            _mockGetClaimantByIdUseCase.Setup(x => x.Execute(testAcademyId)).Returns(singleClaimantInfo);
            var response = _classUnderTest.ViewRecord(testAcademyId) as OkObjectResult;

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
            response.Value.Should().BeEquivalentTo(singleClaimantInfo);
        }

    }
}
