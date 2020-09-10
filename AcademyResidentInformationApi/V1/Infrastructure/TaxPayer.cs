using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademyResidentInformationApi.V1.Infrastructure
{
    [Table("ctaccount", Schema = "dbo")]
    public class TaxPayer
    {
        [Key]
        [Column("account_ref")]
        public int AccountRef { get; set; }

        [Column("lead_liab_forename")]
        [MaxLength(32)]
        public string FirstName { get; set; }

        [Column("lead_liab_surname")]
        [MaxLength(32)]
        public string LastName { get; set; }
    }
}
