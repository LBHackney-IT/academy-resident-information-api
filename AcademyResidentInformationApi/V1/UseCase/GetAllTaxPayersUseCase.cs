using AcademyResidentInformationApi.V1.Boundary.Requests;
using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Gateways;
using AcademyResidentInformationApi.V1.UseCase.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace AcademyResidentInformationApi.V1.UseCase
{
    public class GetAllTaxPayersUseCase : IGetAllTaxPayersUseCase
    {
        private readonly IAcademyGateway _academyGateway;
        public GetAllTaxPayersUseCase(IAcademyGateway academyGateway)
        {
            _academyGateway = academyGateway;
        }

        public TaxPayerInformationList Execute(QueryParameters qp)
        {
            var limit = qp.Limit < 10 ? 10 : qp.Limit;
            limit = qp.Limit > 100 ? 100 : limit;
            var taxPayers = _academyGateway.GetAllTaxPayers(qp.Cursor, limit, qp.FirstName, qp.LastName, qp.Postcode, qp.Address).ToResponse();
            return new TaxPayerInformationList
            {
                TaxPayers = taxPayers,
                NextCursor = GetNextCursor(taxPayers, limit)
            };
        }
        private static string GetNextCursor(List<TaxPayerInformationResponse> taxPayers, int limit)
        {
            return taxPayers.Count == limit ? taxPayers.Max(r => r.AccountRef).ToString() : null;
        }
    }
}
