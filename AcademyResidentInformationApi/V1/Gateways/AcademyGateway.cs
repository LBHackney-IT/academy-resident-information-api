using System.Collections.Generic;
using System.Linq;
using AcademyResidentInformationApi.V1.Boundary.Responses;
using AcademyResidentInformationApi.V1.Domain;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Infrastructure;

namespace AcademyResidentInformationApi.V1.Gateways
{
    public class AcademyGateway : IAcademyGateway
    {
        private readonly AcademyContext _academyContext;


        public AcademyGateway(AcademyContext academyContext)
        {
            _academyContext = academyContext;
        }

        // public Entity GetEntityById(int id)
        // {
        //     var result = _academyContext.DatabaseEntities.Find(id);

        //     return (result != null) ?
        //         result.ToDomain() :
        //         null;
        // }

        public List<ResidentInformation> GetAllResidents()
        {
            var persons = _academyContext.Persons
                .ToList();

            var personDomain = persons.ToDomain();


            // foreach (var person in personDomain)
            // {
            //     var addressesForPerson = _academyContext.Addresses.Where(a => a.Id.ToString() == person.Id);
            //     person.AddressList = addressesForPerson.Any() ? addressesForPerson.Select(s => s.ToDomain()).ToList() : null;
            //     // var phoneNumbersForPerson = GetPhoneNumbersByPersonId(Int32.Parse(person.MosaicId));
            //     // person.PhoneNumberList = phoneNumbersForPerson.Any() ? phoneNumbersForPerson : null;
            //     // person.Uprn = GetMostRecentUprn(addressesForPerson);
            // }

            return personDomain;
        }
    }
}
