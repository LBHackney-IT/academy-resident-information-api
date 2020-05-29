using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademyResidentInformationApi.V1.Infrastructure
{
    [Table("hbhousehold")]
    public class Address
    {
        [ForeignKey("ClaimId")]
        public Person Person { get; set; }

        [Column("claim_id")]
        [MaxLength(32)]
        [Key]
        public int ClaimId { get; set; }

        [Column("addr1")]
        [MaxLength(116)]
        [Key]
        public string AddressLine1 { get; set; }

        [Column("addr2")]
        [MaxLength(116)]
        [Key]
        public string AddressLine2 { get; set; }

        [Column("addr3")]
        [MaxLength(116)]
        [Key]
        public string AddressLine3 { get; set; }

        [Column("addr4")]
        [MaxLength(116)]
        [Key]
        public string AddressLine4 { get; set; }

        [Column("post_code")]
        [MaxLength(16)]
        [Key]
        public string PostCode { get; set; }


    }
}
