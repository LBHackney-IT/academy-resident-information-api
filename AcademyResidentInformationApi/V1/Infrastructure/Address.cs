using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademyResidentInformationApi.V1.Infrastructure
{
    [Table("hbhousehold")]
    public class Address
    {
        public Person Person { get; set; }

        [Column("claim_id")]
        public int ClaimId { get; set; }

        [ForeignKey("ClaimId")]
        public Claim Claim { get; set; }

        [Column("house_id")]
        public int HouseId { get; set; }

        [Column("addr1")]
        [MaxLength(35)]
        public string AddressLine1 { get; set; }

        [Column("addr2")]
        [MaxLength(35)]
        public string AddressLine2 { get; set; }

        [Column("addr3")]
        [MaxLength(32)]
        public string AddressLine3 { get; set; }

        [Column("addr4")]
        [MaxLength(32)]
        public string AddressLine4 { get; set; }

        [Column("post_code")]
        [MaxLength(10)]
        public string PostCode { get; set; }
    }
}
