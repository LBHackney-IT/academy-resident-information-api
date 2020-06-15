using System.Collections.Generic;

namespace AcademyResidentInformationApi.V1.Boundary.Responses
{
    public class ClaimantInformation
    {
        public string AcademyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }

        public string NINumber { get; set; }
        public List<Address> AddressList { get; set; }
    }
}
