using System;

namespace AcademyResidentInformationApi.V1.Domain
{
    public class InvalidCursorException : Exception
    {
        public InvalidCursorException(string message) : base(message)
        { }

        public InvalidCursorException()
        { }
    }
}
