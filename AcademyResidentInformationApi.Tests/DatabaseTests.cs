using AcademyResidentInformationApi.V1.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NUnit.Framework;

namespace AcademyResidentInformationApi.Tests
{
    [NonParallelizable]
    [TestFixture]
    public class DatabaseTests
    {
        protected AcademyContext AcademyContext { get; set; }
        private IDbContextTransaction _transaction;
        private DbContextOptionsBuilder _builder;


        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            _builder = new DbContextOptionsBuilder();
            _builder.UseNpgsql(ConnectionString.TestDatabase());
        }

        [SetUp]
        public void Setup()
        {
            AcademyContext = new AcademyContext(_builder.Options);
            AcademyContext.Database.EnsureCreated();
            AcademyContext.TaxPayers.RemoveRange(AcademyContext.TaxPayers);
            AcademyContext.Persons.RemoveRange(AcademyContext.Persons);
            _transaction = AcademyContext.Database.BeginTransaction();
        }

        [TearDown]
        public void TearDown()
        {
            _transaction.Rollback();
            _transaction.Dispose();
        }
    }
}
