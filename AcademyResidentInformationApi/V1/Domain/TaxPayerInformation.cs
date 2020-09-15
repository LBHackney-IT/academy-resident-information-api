using System.Collections.Generic;

namespace AcademyResidentInformationApi.V1.Domain
{
    public class TaxPayerInformation
    {
        public int AccountRef { get; set; }
        public string Uprn { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> EmailList { get; set; }
        public List<string> PhoneNumberList { get; set; }
        public Address TaxPayerAddress { get; set; }
    }
}
