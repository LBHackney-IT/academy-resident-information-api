using System.Collections.Generic;

namespace AcademyResidentInformationApi.V1.Boundary.Responses
{
    public class TaxPayerInformationList
    {
        public List<TaxPayerInformationResponse> TaxPayers { get; set; }
        public string NextCursor { get; set; }
    }
}
