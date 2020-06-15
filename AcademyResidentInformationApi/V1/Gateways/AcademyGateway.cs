using System.Collections.Generic;
using System.Globalization;
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

        public List<ClaimantInformation> GetAllClaimants(string postcode = null, string address = null)
        {
            var addressesWithNoFilters = _academyContext.Addresses
                .Include(p => p.Person)
                .ToList();

            var peopleWithAddresses = addressesWithNoFilters
                .GroupBy(address => address.Person, MapPersonAndAddressesToClaimantInformation)
                .ToList();

            var peopleWithNoAddress = string.IsNullOrEmpty(postcode) && string.IsNullOrEmpty(address)
                ? QueryPeopleWithNoAddressByName(addressesWithNoFilters)
                : new List<ClaimantInformation>();

            var allClaimantInfo = peopleWithAddresses.Concat(peopleWithNoAddress).ToList();

            return allClaimantInfo;
        }

        public ClaimantInformation GetClaimantById(int claimId, int personRef)
        {
            var databaseRecord = _academyContext.Persons.Find(claimId, personRef);
            if (databaseRecord == null) return null;

            var addressesForPerson = _academyContext.Addresses.Where(a => (a.ClaimId == databaseRecord.Id) && (a.HouseId == databaseRecord.HouseId));
            var singleClaimant = MapPersonAndAddressesToClaimantInformation(databaseRecord, addressesForPerson);

            return singleClaimant;
        }

        private static ClaimantInformation MapPersonAndAddressesToClaimantInformation(Person person, IEnumerable<Address> addresses)
        {
            var claimant = person.ToDomain();
            var addressesDomain = addresses.Select(address => address.ToDomain()).ToList();
            claimant.AddressList = addressesDomain.Any()
                ? addressesDomain
                : null;
            return claimant;
        }

        private List<ClaimantInformation> QueryPeopleWithNoAddressByName(List<Address> addressesWithNoFilters)
        {
            return _academyContext.Persons
                .ToList()
                .Where(p => addressesWithNoFilters.All(add => add.ClaimId != p.Id))
                .Select(person =>
                {
                    var domainPerson = person.ToDomain();
                    domainPerson.AddressList = null;
                    return domainPerson;
                }).ToList();
        }
    }
}
