using System;
using System.Collections.Generic;
using System.Linq;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Gateways;
using AcademyResidentInformationApi.V1.UseCase;
using AutoFixture;
using FluentAssertions;
using Moq;
using AcademyResidentInformationApi.V1.Boundary.Requests;
using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Domain;
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
            var stubbedClaimants = _fixture.CreateMany<ClaimantInformation>().ToList();

            _mockAcademyGateway.Setup(x =>
                x.GetAllClaimants(It.IsAny<Cursor>(), 20, "ciasom", "tessellate", "E8 1DY", "1 Montage street"))
                .Returns(stubbedClaimants);

            var cqp = new ClaimantQueryParam
            {
                FirstName = "ciasom",
                LastName = "tessellate",
                Postcode = "E8 1DY",
                Address = "1 Montage street"
            };
            var response = _classUnderTest.Execute(cqp, null, 20);

            response.Should().NotBeNull();
            response.Claimants.Should().BeEquivalentTo(stubbedClaimants.ToResponse());
        }

        [Test]
        public void ExecuteHasAMaximumNumberOfItemsReturned()
        {
            var stubbedClaimants = _fixture.CreateMany<ClaimantInformation>(101);

            _mockAcademyGateway.Setup(x =>
                x.GetAllClaimants(It.IsAny<Cursor>(), 100, null, null, null, null))
                .Returns(stubbedClaimants.ToList()).Verifiable();

            _classUnderTest.Execute(new ClaimantQueryParam(), null, 101);

            _mockAcademyGateway.Verify();
        }

        [Test]
        public void ExecuteHasAMinimumNumberOfItemsReturned()
        {
            var stubbedClaimants = _fixture.CreateMany<ClaimantInformation>(20);

            _mockAcademyGateway.Setup(x =>
                x.GetAllClaimants(It.IsAny<Cursor>(), 10, null, null, null, null))
                .Returns(stubbedClaimants.ToList()).Verifiable();

            _classUnderTest.Execute(new ClaimantQueryParam(), null, 2);

            _mockAcademyGateway.Verify();
        }

        [Test]
        public void ExecuteWillPassADefaultCursorIfOneIsNotGiven()
        {
            var defaultCursor = new Cursor { ClaimId = 0, HouseId = 0, MemberId = 0 };
            _mockAcademyGateway.Setup(x => x.GetAllClaimants(CheckCursorIs(defaultCursor), 20, null, null, null, null))
                .Returns(new List<ClaimantInformation>());
            _classUnderTest.Execute(new ClaimantQueryParam(), null, 20);
            _mockAcademyGateway.Verify();
        }

        [Test]
        public void ExecuteWillDeconstructACursorPassedToIt()
        {
            var expectedCursor = new Cursor { ClaimId = 3400002, HouseId = 34, MemberId = 2 };
            _mockAcademyGateway.Setup(x => x.GetAllClaimants(CheckCursorIs(expectedCursor), 20, null, null, null, null))
                .Returns(new List<ClaimantInformation>());
            _classUnderTest.Execute(new ClaimantQueryParam(), "3400002-34-2", 20);
            _mockAcademyGateway.Verify();
        }

        [Test]
        public void ExecuteWillConstructTheNextCursorAndReturnIt()
        {
            var lastReturnedClaimant = _fixture.Build<ClaimantInformation>()
                .With(c => c.ClaimId, 3400002)
                .With(c => c.HouseId, 34)
                .With(c => c.MemberId, 2)
                .Create();
            var stubbedClaimants = _fixture.CreateMany<ClaimantInformation>(19).ToList();
            stubbedClaimants.Add(lastReturnedClaimant);

            _mockAcademyGateway.Setup(x => x.GetAllClaimants(It.IsAny<Cursor>(), 20, null, null, null, null))
                .Returns(stubbedClaimants);
            _classUnderTest.Execute(new ClaimantQueryParam(), null, 20).NextCursor.Should().Be("3400002-34-2");
        }

        [Test]
        public void IfTheGivenCursorHasTheWrongFormatItThrows()
        {
            Func<ClaimantInformationList> testDelegate = () =>
                _classUnderTest.Execute(new ClaimantQueryParam(), "222/78", 20);
            testDelegate.Should().Throw<InvalidCursorException>("The cursor provided is in the wrong format, please use the cursor provided in the previous response or if this is the first request leave it blank");
        }

        private static Cursor CheckCursorIs(Cursor cursor)
        {
            return It.Is<Cursor>(c =>
                c.ClaimId == cursor.ClaimId && c.HouseId == cursor.HouseId && c.MemberId == cursor.MemberId);
        }
    }
}

