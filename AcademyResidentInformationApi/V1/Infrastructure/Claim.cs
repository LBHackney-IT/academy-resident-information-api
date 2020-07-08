using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademyResidentInformationApi.V1.Infrastructure
{
    [Table("hbclaim", Schema = "dbo")]
    public class Claim
    {
        [Column("claim_id")]
        [Key]
        public int? ClaimId { get; set; }

        [Column("check_digit", TypeName = "varchar")]
        [MaxLength(1)]
        public string CheckDigit { get; set; }
    }
}
