using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademyResidentInformationApi.V1.Infrastructure
{
    [Table("syemail", Schema = "dbo")]
    public class Email
    {
        [Column("reference_id")]
        public int ReferenceId { get; set; }

        [Column("person_type")]
        public short PersonType { get; set; }

        [Column("person_type_seq_no")]
        public short PersonTypeSequenceNumber { get; set; }

        [Column("email_addr")]
        [MaxLength(128)]
        public string EmailAddress { get; set; }
    }
}
