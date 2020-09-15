using AcademyResidentInformationApi.V1.Boundary.Responses;

namespace AcademyResidentInformationApi.V1.UseCase
{
    public interface IGetTaxPayerByIdUseCase
    {
        TaxPayerInformationResponse Execute(int accountRef);
    }
}
