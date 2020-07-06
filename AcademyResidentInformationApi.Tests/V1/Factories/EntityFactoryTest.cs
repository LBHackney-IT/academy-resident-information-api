using System.Globalization;
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
            var personRecord = TestHelper.CreateDatabasePersonEntity();
            var domain = personRecord.ToDomain();
            domain.Should().BeEquivalentTo(new ClaimantInformation
            {
                ClaimId = personRecord.ClaimId,
                PersonRef = personRecord.PersonRef,
                FirstName = personRecord.FirstName,
                LastName = personRecord.LastName,
                DateOfBirth = personRecord.DateOfBirth.ToString("O", CultureInfo.InvariantCulture),
                NINumber = personRecord.NINumber
            });
        }
    }
}
