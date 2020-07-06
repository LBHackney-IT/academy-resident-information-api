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
            var addressesFilteredByPostcode = _academyContext.Addresses
                .Include(p => p.Person)
                .Where(a => string.IsNullOrEmpty(address) || a.AddressLine1.ToLower().Replace(" ", "").Contains(StripString(address)))
                .Where(a => string.IsNullOrEmpty(postcode) || a.PostCode.ToLower().Replace(" ", "").Equals(StripString(postcode)))
                .Where(a => string.IsNullOrEmpty(firstname) || a.Person.FirstName.ToLower().Replace(" ", "").Contains(StripString(firstname)))
                .Where(a => string.IsNullOrEmpty(lastname) || a.Person.LastName.ToLower().Replace(" ", "").Contains(StripString(lastname)))
                .ToList();

            var peopleWithAddresses = addressesFilteredByPostcode
                .Select(address =>
                    {
                        var person = address.Person.ToDomain();
                        person.ClaimantAddress = address.ToDomain();
                        return person;
                    })
                .ToList();

            return peopleWithAddresses;
        }
        private static string StripString(string str)
        {
            return str?.ToLower().Replace(" ", "");
        }

        public ClaimantInformation GetClaimantById(int claimId, int personRef)
        {
            //Retrieve the first record or null
            var databaseRecord = _academyContext.Persons.Where(r => r.ClaimId == claimId && r.PersonRef == personRef)
                .FirstOrDefault();
            if (databaseRecord == null) return null;

            var addressesForPerson = _academyContext.Addresses.Where(a => (a.ClaimId == databaseRecord.ClaimId) && (a.HouseId == databaseRecord.HouseId));
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
