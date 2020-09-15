using System;

namespace AcademyResidentInformationApi.V1.Domain
{
    public class TaxPayerNotFoundException : Exception
    {
        public TaxPayerNotFoundException() { }
        public TaxPayerNotFoundException(string message) : base(message)
        { }
    }
}
