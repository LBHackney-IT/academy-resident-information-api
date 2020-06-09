using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ResidentInformation = AcademyResidentInformationApi.V1.Domain.ResidentInformation;

namespace AcademyResidentInformationApi.V1.Gateways
{
    public class AcademyGateway : IAcademyGateway
    {
        private readonly AcademyContext _academyContext;

        public AcademyGateway(AcademyContext academyContext)
        {
            _academyContext = academyContext;
        }

        public List<ResidentInformation> GetAllResidents(string postcode = null, string address = null)
        {
            var addressesWithNoFilters = _academyContext.Addresses
                .Include(p => p.Person)
                .ToList();

            var peopleWithAddresses = addressesWithNoFilters
                .GroupBy(address => address.Person, MapPersonAndAddressesToResidentInformation)
                .ToList();

            var peopleWithNoAddress = string.IsNullOrEmpty(postcode) && string.IsNullOrEmpty(address)
                ? QueryPeopleWithNoAddressByName(addressesWithNoFilters)
                : new List<ResidentInformation>();

            var allResidentInfo = peopleWithAddresses.Concat(peopleWithNoAddress).ToList();

            return allResidentInfo;
        }

        public ResidentInformation GetResidentById(int claimId, int personRef)
        {
            var databaseRecord = _academyContext.Persons.Find(claimId, personRef);
            if (databaseRecord == null) return null;

            var addressesForPerson = _academyContext.Addresses.Where(a => (a.ClaimId == databaseRecord.Id) && (a.HouseId == databaseRecord.HouseId));
            var singleResident = MapPersonAndAddressesToResidentInformation(databaseRecord, addressesForPerson);

            return singleResident;
        }

        private static ResidentInformation MapPersonAndAddressesToResidentInformation(Person person, IEnumerable<Address> addresses)
        {
            var resident = person.ToDomain();
            var addressesDomain = addresses.Select(address => address.ToDomain()).ToList();
            resident.AddressList = addressesDomain.Any()
                ? addressesDomain
                : null;
            return resident;
        }

        private List<ResidentInformation> QueryPeopleWithNoAddressByName(List<Address> addressesWithNoFilters)
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
