using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademyResidentInformationApi.V1.Infrastructure
{

    [Table("syphone", Schema = "dbo")]
    public class PhoneNumber
    {
        [Key]
        [Column("ref")]
        public string Reference { get; set; }

        [Column("phonenum1")]
        public string Number1 { get; set; }

        [Column("phonenum2")]
        public string Number2 { get; set; }

        [Column("phonenum3")]
        public string Number3 { get; set; }

        [Column("phonenum4")]
        public string Number4 { get; set; }
    }
}
