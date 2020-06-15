using System.Collections.Generic;

namespace AcademyResidentInformationApi.V1.Boundary.Responses
{
    public class ClaimantInformation
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public List<Address> AddressList { get; set; }
    }
}
