using AcademyResidentInformationApi.V1.Domain;
using AcademyResidentInformationApi.V1.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AcademyResidentInformationApi.V1.Infrastructure
{

    public class AcademyContext : DbContext
    {
        public AcademyContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // composite primary key for Address table
            modelBuilder.Entity<Address>()
                .HasKey(address => new
                {
                    address.ClaimId,
                    address.HouseId
                });

            // composite primary key for Person table
            modelBuilder.Entity<Person>()
                .HasKey(person => new
                {
                    person.ClaimId,
                    person.MemberId,
                    person.HouseId
                });

            // composite foreign key for Address targeting unique columns in the Person table
            modelBuilder.Entity<Person>()
                .HasOne(x => x.Address)
                .WithOne(x => x.Person)
                .HasPrincipalKey<Person>(x => new
                {
                    x.ClaimId,
                    x.HouseId
                });
        }
    }
}
