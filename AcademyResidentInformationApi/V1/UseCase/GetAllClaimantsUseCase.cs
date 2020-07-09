using System;
using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Gateways;
using AcademyResidentInformationApi.V1.UseCase.Interfaces;
using AcademyResidentInformationApi.V1.Boundary.Requests;
using AcademyResidentInformationApi.V1.Domain;

namespace AcademyResidentInformationApi.V1.UseCase
{
    public class GetAllClaimantsUseCase : IGetAllClaimantsUseCase
    {
        private readonly IAcademyGateway _academyGateway;
        private IValidatePostcode _validatePostcode;
        public GetAllClaimantsUseCase(IAcademyGateway academyGateway)
        {
            _academyGateway = academyGateway;
            _validatePostcode = new ValidatePostcode();
        }

        public ClaimantInformationList Execute(ClaimantQueryParam cqp, int cursor, int limit)
        {
            //Check if the query parameter includes a value for postcode
            if (!string.IsNullOrWhiteSpace(cqp.Postcode))
                CheckPostCodeValid(cqp.Postcode);

            limit = limit < 10 ? 10 : limit;
            limit = limit > 100 ? 100 : limit;

            var claimants = _academyGateway.GetAllClaimants(cursor, limit, cqp.FirstName, cqp.LastName, cqp.Postcode, cqp.Address);
            return new ClaimantInformationList
            {
                Claimants = claimants.ToResponse()
            };
        }


        private void CheckPostCodeValid(string postcode)
        {
            var validPostcode = _validatePostcode.Execute(postcode);
            if (!validPostcode)
                throw new InvalidQueryParameterException("The Postcode given does not have a valid format");
        }
    }
}
