using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyResidentInformationApi.V1.UseCase.Interfaces
{
    public interface IValidatePostcode
    {
        bool Execute(string postcode);
    }
}
