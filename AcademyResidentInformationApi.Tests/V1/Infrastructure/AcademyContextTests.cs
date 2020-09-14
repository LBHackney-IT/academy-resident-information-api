using System.Linq;
using AcademyResidentInformationApi.Tests.V1.Helper;
using NUnit.Framework;

namespace AcademyResidentInformationApi.Tests.V1.Infrastructure
{
    [TestFixture]
    public class AcademyContextTests : DatabaseTests
    {
        [Test]
        public void CanGetADatabaseEntity()
        {
            var databaseEntity = TestHelper.CreateDatabaseClaimantEntity();

            AcademyContext.Add(databaseEntity);
            AcademyContext.SaveChanges();

            var result = AcademyContext.Persons.ToList().FirstOrDefault();

            Assert.AreEqual(result, databaseEntity);
        }
    }
}
