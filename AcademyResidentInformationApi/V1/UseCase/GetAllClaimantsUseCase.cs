using System;
using System.Linq;
using AcademyResidentInformationApi.V1.Boundary.Requests;
using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Domain;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Gateways;
using AcademyResidentInformationApi.V1.UseCase.Interfaces;

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

        public ClaimantInformationList Execute(QueryParameters qp, string cursor, int limit)
        {
            //Check if the query parameter includes a value for postcode
            if (!string.IsNullOrWhiteSpace(qp.Postcode))
                CheckPostCodeValid(qp.Postcode);

            limit = limit < 10 ? 10 : limit;
            limit = limit > 100 ? 100 : limit;


            var claimants = _academyGateway.GetAllClaimants(DeconstructCursor(cursor), limit, qp.FirstName, qp.LastName, qp.Postcode, qp.Address);

            var lastClaimant = claimants.LastOrDefault();
            var nextCursor = claimants.Count == limit ? $"{lastClaimant.ClaimId}-{lastClaimant.HouseId}-{lastClaimant.MemberId}" : "";
            return new ClaimantInformationList
            {
                Claimants = claimants.ToResponse(),
                NextCursor = nextCursor
            };
        }

        private static Cursor DeconstructCursor(string cursor)
        {
            if (cursor == null) return new Cursor { ClaimId = 0, HouseId = 0, MemberId = 0 };
            try
            {
                var values = cursor.Split('-');

                return new Cursor
                {
                    ClaimId = Convert.ToInt32(values.ElementAt(0)),
                    HouseId = Convert.ToInt32(values.ElementAt(1)),
                    MemberId = Convert.ToInt32(values.ElementAt(2)),
                };
            }
            catch (Exception)
            {
                throw new InvalidCursorException("The cursor provided is in the wrong format, please use the cursor provided in the previous response or if this is the first request leave it blank");
            }
        }

        private void CheckPostCodeValid(string postcode)
        {
            var validPostcode = _validatePostcode.Execute(postcode);
            if (!validPostcode)
                throw new InvalidQueryParameterException("The Postcode given does not have a valid format");
        }
    }
}
