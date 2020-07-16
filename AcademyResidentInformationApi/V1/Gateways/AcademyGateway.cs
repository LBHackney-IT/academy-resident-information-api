using System.Collections.Generic;
using System.Linq;
using AcademyResidentInformationApi.V1.Domain;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Address = AcademyResidentInformationApi.V1.Infrastructure.Address;
using ClaimantInformation = AcademyResidentInformationApi.V1.Domain.ClaimantInformation;

namespace AcademyResidentInformationApi.V1.Gateways
{
    public class AcademyGateway : IAcademyGateway
    {
        private readonly AcademyContext _academyContext;

        public AcademyGateway(AcademyContext academyContext)
        {
            _academyContext = academyContext;
        }

        public List<ClaimantInformation> GetAllClaimants(Cursor cursor, int limit, string firstname = null,
            string lastname = null, string postcode = null, string address = null)
        {
            var firstNameSearchPattern = GetSearchPattern(firstname);
            var lastNameSearchPattern = GetSearchPattern(lastname);
            var addressSearchPattern = GetSearchPattern(address);
            var postcodeSearchPattern = GetSearchPattern(postcode);
            return (
                from person in _academyContext.Persons
                join a in _academyContext.Addresses on new { person.ClaimId, person.HouseId } equals new { a.ClaimId, a.HouseId }
                join c in _academyContext.Claims on person.ClaimId equals c.ClaimId
                where a.ToDate == "2099-12-31 00:00:00.0000000"
                where string.IsNullOrEmpty(address) || EF.Functions.ILike(a.AddressLine1.Replace(" ", ""), addressSearchPattern)
                where string.IsNullOrEmpty(postcode) || EF.Functions.ILike(a.PostCode.Replace(" ", ""), postcodeSearchPattern)
                where string.IsNullOrEmpty(firstname) || EF.Functions.ILike(person.FirstName, firstNameSearchPattern)
                where string.IsNullOrEmpty(lastname) || EF.Functions.ILike(person.LastName, lastNameSearchPattern)
                where person.ClaimId > cursor.ClaimId || person.HouseId > cursor.HouseId || person.MemberId > cursor.MemberId
                orderby person.ClaimId, person.HouseId, person.MemberId
                select new Person
                {
                    Address = a,
                    Claim = c,
                    Title = person.Title,
                    FirstName = person.FirstName,
                    FullName = person.FullName,
                    ClaimId = person.ClaimId,
                    HouseId = person.HouseId,
                    LastName = person.LastName,
                    MemberId = person.MemberId,
                    PersonRef = person.PersonRef,
                    DateOfBirth = person.DateOfBirth,
                    NINumber = person.NINumber
                }
                ).Take(limit).ToList().ToDomain();
        }
        private static string GetSearchPattern(string str)
        {
            return $"%{str?.Replace(" ", "")}%";
        }

        public ClaimantInformation GetClaimantById(int claimId, int personRef)
        {
            var databaseRecord = _academyContext.Persons
                .Include(p => p.Claim)
                .Join(_academyContext.Addresses, person => new { person.HouseId, person.ClaimId },
                    add => new { add.HouseId, add.ClaimId }, (person, address) => new { address, person })
                .Where(r => r.person.ClaimId == claimId && r.person.PersonRef == personRef)
                .FirstOrDefault(r => r.address.ToDate == "2099-12-31 00:00:00.0000000");

            return databaseRecord == null
                ? null
                : MapPersonAndAddressesToClaimantInformation(databaseRecord.person, databaseRecord.address);
        }

        private static ClaimantInformation MapPersonAndAddressesToClaimantInformation(Person person, Address address)
        {
            var claimant = person.ToDomain();
            claimant.ClaimantAddress = address.ToDomain();
            return claimant;
        }
    }
}
