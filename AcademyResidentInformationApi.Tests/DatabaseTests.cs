using AcademyResidentInformationApi.V1.Infrastructure;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace AcademyResidentInformationApi.Tests
{
    [TestFixture]
    public class DatabaseTests
    {
        protected AcademyContext AcademyContext { get; set; }

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseNpgsql(ConnectionString.TestDatabase());
            AcademyContext = new AcademyContext(builder.Options);

            AcademyContext.Database.EnsureCreated();
            AcademyContext.Database.BeginTransaction();
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
            AcademyContext.Database.RollbackTransaction();
        }
    }
}
