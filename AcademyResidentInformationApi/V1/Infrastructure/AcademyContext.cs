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
            modelBuilder.Entity<Address>()
                .HasKey(address => new
                {
                    address.ClaimId,
                    address.HouseId
                });
        }
    }
}
