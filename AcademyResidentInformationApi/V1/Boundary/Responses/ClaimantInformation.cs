using System.Collections.Generic;

namespace AcademyResidentInformationApi.V1.Boundary.Responses
{
    public class ClaimantInformation
    {
        public int ClaimId { get; set; }
        public string CheckDigit { get; set; }
        public int PersonRef { get; set; }
        public string Title { get; set; }
        public int? StatusIndicator { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }

        public string NINumber { get; set; }
        public Address ClaimantAddress { get; set; }
    }
}
