using System.ComponentModel.DataAnnotations.Schema;

namespace AcademyResidentInformationApi.V1.Infrastructure
{
    [Table("ctproperty", Schema = "dbo")]
    public class CouncilProperty
    {
        [Column("property_ref")]
        public string PropertyRef { get; set; }

        [Column("addr1")]
        public string AddressLine1 { get; set; }

        [Column("addr2")]
        public string AddressLine2 { get; set; }

        [Column("addr3")]
        public string AddressLine3 { get; set; }

        [Column("addr4")]
        public string AddressLine4 { get; set; }

        [Column("postcode")]
        public string PostCode { get; set; }

        [Column("uprn")]
        public string Uprn { get; set; }
    }
}
