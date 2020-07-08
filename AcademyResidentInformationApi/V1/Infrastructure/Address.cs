using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademyResidentInformationApi.V1.Infrastructure
{
    [Table("hbhousehold", Schema = "dbo")]
    public class Address
    {
        public Person Person { get; set; }

        [Column("claim_id")]
        public int? ClaimId { get; set; }

        [ForeignKey("ClaimId")]
        public Claim Claim { get; set; }

        [Column("house_id")]
        public int? HouseId { get; set; }

        [Column("addr1", TypeName = "varchar")]
        [MaxLength(35)]
        public string AddressLine1 { get; set; }

        [Column("addr2", TypeName = "varchar")]
        [MaxLength(35)]
        public string AddressLine2 { get; set; }

        [Column("addr3", TypeName = "varchar")]
        [MaxLength(32)]
        public string AddressLine3 { get; set; }

        [Column("addr4", TypeName = "varchar")]
        [MaxLength(32)]
        public string AddressLine4 { get; set; }

        [Column("post_code", TypeName = "varchar")]
        [MaxLength(10)]
        public string PostCode { get; set; }
    }
}
