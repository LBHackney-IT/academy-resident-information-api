using System.ComponentModel.DataAnnotations.Schema;

namespace AcademyResidentInformationApi.V1.Infrastructure
{
    [Table("syemail", Schema = "dbo")]
    public class Email
    {
        [Column("reference_id")]
        public int ReferenceId { get; set; }

        [Column("email_addr")]
        public string EmailAddress { get; set; }
    }
}
