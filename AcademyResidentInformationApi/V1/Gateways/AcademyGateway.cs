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
            var people = _academyContext.Persons
                .Include(p => p.Address)
                .Include(p => p.Claim)
                .Where(a => string.IsNullOrEmpty(address) || a.Address.AddressLine1.ToLower().Replace(" ", "").Contains(StripString(address)))
                .Where(a => string.IsNullOrEmpty(postcode) || a.Address.PostCode.ToLower().Replace(" ", "").Equals(StripString(postcode)))
                .Where(a => string.IsNullOrEmpty(firstname) || a.FirstName.ToLower().Replace(" ", "").Contains(StripString(firstname)))
                .Where(a => string.IsNullOrEmpty(lastname) || a.LastName.ToLower().Replace(" ", "").Contains(StripString(lastname)))
                .OrderBy(p => p.ClaimId)
                .ThenBy(p => p.HouseId)
                .ThenBy(p => p.MemberId)
                .Skip(cursor)
                .Take(limit)
                .ToList();

            var addressesFilteredByPostcode = _academyContext.Addresses
                .Include(p => p.Person)
                .Include(p => p.Claim)
                .Where(a => string.IsNullOrEmpty(address) || a.AddressLine1.ToLower().Replace(" ", "").Contains(StripString(address)))
                .Where(a => string.IsNullOrEmpty(postcode) || a.PostCode.ToLower().Replace(" ", "").Equals(StripString(postcode)))
                .Where(a => string.IsNullOrEmpty(firstname) || a.Person.FirstName.ToLower().Replace(" ", "").Contains(StripString(firstname)))
                .Where(a => string.IsNullOrEmpty(lastname) || a.Person.LastName.ToLower().Replace(" ", "").Contains(StripString(lastname)))
                .Skip(cursor)
                .Take(limit)
                .ToList();

            var domainPeople = people
                .Select(person =>
                    {
                        var domain = person.ToDomain();
                        domain.ClaimantAddress = person.Address.ToDomain();
                        return domain;
                    })
                .ToList();

            return domainPeople;
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
