using System;

namespace AcademyResidentInformationApi.V1.Domain
{
    public class ClaimantNotFoundException : Exception
    {
        public ClaimantNotFoundException() { }
        public ClaimantNotFoundException(string message) : base(message)
        { }
    }
}
