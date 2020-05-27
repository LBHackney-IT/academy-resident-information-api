using Microsoft.EntityFrameworkCore;

namespace AcademyResidentInformationApi.V1.Infrastructure
{
    public interface IDatabaseContext
    {
        DbSet<DatabaseEntity> DatabaseEntities { get; set; }
    }
}
