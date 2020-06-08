using Microsoft.AspNetCore.Mvc;

namespace AcademyResidentInformationApi.V1.Boundary.Requests
{
    public class ClaimantQueryParam
    {
        [FromQuery(Name = "first_name")]
        public string FirstName { get; set; }

        [FromQuery(Name = "last_name")]
        public string LastName { get; set; }

        [FromQuery(Name = "address")]
        public string Address { get; set; }

        [FromQuery(Name = "postcode")]
        public string Postcode { get; set; }
    }
}
