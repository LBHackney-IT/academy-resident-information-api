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

        public DbSet<Claim> Claims { get; set; }
        public DbSet<TaxPayer> TaxPayers { get; set; }
        public DbSet<CouncilProperty> CouncilProperties { get; set; }
        public DbSet<Occupation> Occupations { get; set; }

        public DbSet<Email> Emails { get; set; }

        public DbSet<PhoneNumber> PhoneNumbers { get; set; }

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

            modelBuilder.Entity<Email>()
                .HasKey(email => new
                {
                    email.PersonType,
                    email.PersonTypeSequenceNumber,
                    email.ReferenceId
                });
        }
    }
}
