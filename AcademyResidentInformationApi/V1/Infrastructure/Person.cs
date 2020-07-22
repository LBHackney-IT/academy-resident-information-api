using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademyResidentInformationApi.V1.Infrastructure
{
    [Table("hbmember", Schema = "dbo")]
    public class Person
    {
        [Column("claim_id")]
        [Key]
        public int? ClaimId { get; set; }

        [ForeignKey("ClaimId")]
        public Claim Claim { get; set; }

        [Column("house_id")]
        public int? HouseId { get; set; }

        [Column("member_id")]
        public int? MemberId { get; set; }

        [Column("person_ref")]
        public int? PersonRef { get; set; }

        [Column("forename", TypeName = "varchar")]
        [MaxLength(32)]
        public string FirstName { get; set; }

        [Column("surname", TypeName = "varchar")]
        [MaxLength(32)]
        public string LastName { get; set; }

        [Column("title", TypeName = "varchar")]
        [MaxLength(4)]
        public string Title { get; set; }

        [Column("find_name", TypeName = "varchar")]
        [MaxLength(32)]
        public string FullName { get; set; }

        [Column("birth_date", TypeName = "varchar")]
        [MaxLength(37)]
        public string DateOfBirth { get; set; }

        [Column("nino")]
        [MaxLength(10)]
        public string NINumber { get; set; }

        public Address Address { get; set; }
    }
}
