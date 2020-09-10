namespace AcademyResidentInformationApi.V1.Domain
{
    public class ClaimantInformation
    {
        public int ClaimId { get; set; }
        public int PersonRef { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string NINumber { get; set; }
        public Address ClaimantAddress { get; set; }
        public string CheckDigit { get; set; }
        public int? StatusIndicator { get; set; }

        public int MemberId { get; set; }
        public int HouseId { get; set; }
    }
}
