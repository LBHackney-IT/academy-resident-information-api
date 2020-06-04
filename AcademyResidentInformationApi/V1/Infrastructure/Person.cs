using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyResidentInformationApi.V1.Infrastructure
{
    [Table("hbmember")]
    public class Person
    {
        [Column("claim_id")]
        [Key]
        public int Id { get; set; }

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
    }
}
