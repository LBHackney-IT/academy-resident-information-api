using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyResidentInformationApi.V1.Infrastructure
{
    [Table("hbmember", Schema = "dbo")]
    public class Person
    {
        [Column("claim_id")]
        [Key]
        public int ClaimId { get; set; }

        [ForeignKey("ClaimId")]
        public Claim Claim { get; set; }

        [Column("house_id")]
        public int HouseId { get; set; }

        [Column("member_id")]
        public int MemberId { get; set; }

        [Column("person_ref")]
        public int PersonRef { get; set; }

        [Column("forename")]
        [MaxLength(32)]
        public string FirstName { get; set; }

        [Column("surname")]
        [MaxLength(32)]
        public string LastName { get; set; }

        [Column("title")]
        [MaxLength(4)]
        public string Title { get; set; }

        [Column("find_name")]
        [MaxLength(32)]
        public string FullName { get; set; }

        [Column("birth_date")]
        public DateTime DateOfBirth { get; set; }

        [Column("nino")]
        [MaxLength(10)]
        public string NINumber { get; set; }

        public Address Address { get; set; }
    }
}
