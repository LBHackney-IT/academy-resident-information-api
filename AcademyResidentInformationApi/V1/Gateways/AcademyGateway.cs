using System.Collections.Generic;
using System.Linq;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Infrastructure;
using Microsoft.EntityFrameworkCore;
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

        public List<ClaimantInformation> GetAllClaimants(int cursor, int limit, string firstname = null,
            string lastname = null, string postcode = null, string address = null)
        {
            return (
                from person in _academyContext.Persons
                join a in _academyContext.Addresses on new { person.ClaimId, person.HouseId } equals new { a.ClaimId, a.HouseId }
                join c in _academyContext.Claims on person.ClaimId equals c.ClaimId
                where string.IsNullOrEmpty(address) || a.AddressLine1.ToLower().Replace(" ", "").Contains(StripString(address))
                where string.IsNullOrEmpty(postcode) || a.PostCode.ToLower().Replace(" ", "").Equals(StripString(postcode))
                where string.IsNullOrEmpty(firstname) || person.FirstName.ToLower().Replace(" ", "").Contains(StripString(firstname))
                where string.IsNullOrEmpty(lastname) || person.LastName.ToLower().Replace(" ", "").Contains(StripString(lastname))
                where a.ToDate == "2099-12-31 00:00:00.0000000"
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
                ).Skip(cursor).Take(limit).ToList().ToDomain();
        }
        private static string StripString(string str)
        {
            return str?.ToLower().Replace(" ", "");
        }

        public ClaimantInformation GetClaimantById(int claimId, int personRef)
        {
            //Retrieve the first record or null
            var databaseRecord = _academyContext.Persons
                .Include(p => p.Claim)
                .FirstOrDefault(r => r.ClaimId == claimId && r.PersonRef == personRef);
            if (databaseRecord == null) return null;

            var addressesForPerson = _academyContext.Addresses.Where(a =>
                (a.ClaimId == databaseRecord.ClaimId) && (a.HouseId == databaseRecord.HouseId));
            var singleClaimant = MapPersonAndAddressesToClaimantInformation(databaseRecord, addressesForPerson);

            return singleClaimant;
        }

        private static ClaimantInformation MapPersonAndAddressesToClaimantInformation(Person person, IEnumerable<Address> addresses)
        {
            var claimant = person.ToDomain();
            var addressesDomain = addresses.Select(address => address.ToDomain()).ToList();
            claimant.ClaimantAddress = addressesDomain.Any() ? addressesDomain.First() : null;
            return claimant;
        }
    }
}
