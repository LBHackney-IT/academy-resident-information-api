using System.Linq;
using AcademyResidentInformationApi.Tests.V1.Helper;
using AcademyResidentInformationApi.V1.Infrastructure;
using NUnit.Framework;

namespace AcademyResidentInformationApi.Tests.V1.Infrastructure
{
    [TestFixture]
    public class DatabaseContextTest : DatabaseTests
    {
        [Test]
        public void CanGetADatabaseEntity()
        {
            DatabaseEntity databaseEntity = DatabaseEntityHelper.CreateDatabaseEntity();

            AcademyContext.Add(databaseEntity);
            AcademyContext.SaveChanges();

            var result = AcademyContext.Persons.ToList().FirstOrDefault();

            Assert.AreEqual(result, databaseEntity);
        }
    }
}
