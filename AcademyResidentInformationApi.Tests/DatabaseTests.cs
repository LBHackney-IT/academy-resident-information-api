using AcademyResidentInformationApi.V1.Infrastructure;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace AcademyResidentInformationApi.Tests
{
    [TestFixture]
    public class DatabaseTests
    {
        protected DatabaseContext DatabaseContext { get; set; }

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseNpgsql(ConnectionString.TestDatabase());
            DatabaseContext = new DatabaseContext(builder.Options);

            DatabaseContext.Database.EnsureCreated();
            DatabaseContext.Database.BeginTransaction();
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
            DatabaseContext.Database.RollbackTransaction();
        }
    }
}
