using AcademyResidentInformationApi.V1.Domain;
using Microsoft.EntityFrameworkCore;
using AcademyResidentInformationApi.V1.Infrastructure;

namespace AcademyResidentInformationApi.V1.Infrastructure
{

    public class AcademyContext : DbContext
    {
        public AcademyContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Address> Addresses { get; set; }

        //public DbSet<TelephoneNumber> TelephoneNumbers { get; set; }
        //Don't think the database hold up-to-date telephone numbers, we may have some for landlords.
    }
}
