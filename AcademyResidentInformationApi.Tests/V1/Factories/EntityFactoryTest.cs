using AcademyResidentInformationApi.Tests.V1.Helper;
using AcademyResidentInformationApi.V1.Domain;
using AcademyResidentInformationApi.V1.Factories;
using FluentAssertions;
using NUnit.Framework;

namespace AcademyResidentInformationApi.Tests.V1.Factories
{
    [TestFixture]
    public class EntityFactoryTest
    {
        [Test]
        public void ItMapsAPersonDatabaseRecordIntoClaimantInformationDomainObject()
        {
            var personRecord = TestHelper.CreateDatabaseClaimantEntity();
            var domain = personRecord.ToDomain();
            domain.Should().BeEquivalentTo(new ClaimantInformation
            {
                ClaimId = personRecord.ClaimId.Value,
                PersonRef = personRecord.PersonRef.Value,
                Title = personRecord.Title,
                FirstName = personRecord.FirstName,
                LastName = personRecord.LastName,
                DateOfBirth = personRecord.DateOfBirth,
                NINumber = personRecord.NINumber,
                HouseId = personRecord.HouseId.Value,
                MemberId = personRecord.MemberId.Value,
            });
        }

        [Test]
        public void ItMapsTheAddressOntoTheClaimantInformationDomainObject()
        {
            var personRecord = TestHelper.CreateDatabaseClaimantEntity();
            var address = personRecord.Address =
                TestHelper.CreateDatabaseAddressForPersonId(personRecord.ClaimId, personRecord.HouseId);
            var domain = personRecord.ToDomain();
            domain.ClaimantAddress.Should().NotBeNull();
            domain.ClaimantAddress.AddressLine1.Should().Be(address.AddressLine1);
            domain.ClaimantAddress.AddressLine2.Should().Be(address.AddressLine2);
            domain.ClaimantAddress.AddressLine3.Should().Be(address.AddressLine3);
            domain.ClaimantAddress.AddressLine4.Should().Be(address.AddressLine4);
            domain.ClaimantAddress.PostCode.Should().Be(address.PostCode);
        }
    }
}
