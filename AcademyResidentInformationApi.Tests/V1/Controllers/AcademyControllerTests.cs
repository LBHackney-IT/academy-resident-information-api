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
        private Mock<IGetAllResidentsUseCase> _mockGetAllResidentsUseCase;

        [SetUp]
        public void SetUp()
        {
            _mockGetAllResidentsUseCase = new Mock<IGetAllResidentsUseCase>();
            _classUnderTest = new AcademyController(_mockGetAllResidentsUseCase.Object);
        }

        [Test]
        public void ListContacts()
        {

            var residentInfo = new List<ResidentInformation>()
            {
                new ResidentInformation()
                {
                    FirstName = "test",
                    LastName = "test",
                    DateOfBirth = "01/01/2020"

                }
            };

            var residentInformationList = new ResidentInformationList()
            {
                Residents = residentInfo
            };

            _mockGetAllResidentsUseCase.Setup(x => x.Execute()).Returns(residentInformationList);
            var response = _classUnderTest.ListContacts() as OkObjectResult;

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
            response.Value.Should().BeEquivalentTo(residentInformationList);
        }

    }
}
