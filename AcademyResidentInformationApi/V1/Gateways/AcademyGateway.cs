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

        public List<ResidentInformation> GetAllResidents(int cursor, int limit, string firstname = null,
            string lastname = null, string postcode = null, string address = null)
        {
            var addressesFilteredByPostcode = _academyContext.Addresses
                .Include(p => p.Person)
                .Where(a => string.IsNullOrEmpty(address) || a.AddressLine1.ToLower().Replace(" ", "").Contains(StripString(address)))
                .Where(a => string.IsNullOrEmpty(postcode) || a.PostCode.ToLower().Replace(" ", "").Equals(StripString(postcode)))
                .Where(a => string.IsNullOrEmpty(firstname) || a.Person.FirstName.ToLower().Replace(" ", "").Equals(StripString(firstname)))
                .Where(a => string.IsNullOrEmpty(lastname) || a.Person.LastName.ToLower().Replace(" ", "").Equals(StripString(lastname)))
                .Where(a => a.Person.Id > cursor)
                .ToList();

            var peopleWithAddresses = addressesFilteredByPostcode
                .Select(address =>
                    {
                        var person = address.Person.ToDomain();
                        person.ResidentAddress = address.ToDomain();
                        return person;
                    })
                .ToList();

            return peopleWithAddresses;
        }
        private static string StripString(string str)
        {
            return str?.ToLower().Replace(" ", "");
        }
    }
}
