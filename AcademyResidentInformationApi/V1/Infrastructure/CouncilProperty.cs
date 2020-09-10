using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademyResidentInformationApi.V1.Infrastructure
{
    [Table("ctproperty", Schema = "dbo")]
    public class CouncilProperty
    {
        [Key]
        [Column("property_ref")]
        [MaxLength(18)]
        public string PropertyRef { get; set; }

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

        [Column("postcode")]
        [MaxLength(8)]
        public string PostCode { get; set; }

        [Column("uprn")]
        [MaxLength(12)]
        public string Uprn { get; set; }
    }
}
