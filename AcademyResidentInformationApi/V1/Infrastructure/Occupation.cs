using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademyResidentInformationApi.V1.Infrastructure
{
    [Table("ctoccupation", Schema = "dbo")]
    public class Occupation
    {
        [Key]
        [Column("account_ref")]
        public int AccountRef { get; set; }

        [Column("property_ref")]
        [MaxLength(18)]
        public string PropertyRef { get; set; }
    }
}
