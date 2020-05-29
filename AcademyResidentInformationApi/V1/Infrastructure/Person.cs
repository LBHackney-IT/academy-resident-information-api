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
        [MaxLength(32)]
        [Key]
        public int Id { get; set; }

        [Column("forename")]
        [MaxLength(32)]
        [Key]
        public string FirstName { get; set; }

        [Column("surname")]
        [MaxLength(32)]
        [Key]
        public string LastName { get; set; }

        [Column("title")]
        [MaxLength(8)]
        public string Title { get; set; }

        [Column("find_name")]
        [MaxLength(62)]
        public string FullName { get; set; }

        [Column("birth_date")]
        public DateTime DateOfBirth { get; set; }

    }
}
