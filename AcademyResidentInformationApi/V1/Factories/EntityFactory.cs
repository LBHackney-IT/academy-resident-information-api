using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AcademyResidentInformationApi.V1.Infrastructure;
using Address = AcademyResidentInformationApi.V1.Domain.Address;
using ClaimantInformation = AcademyResidentInformationApi.V1.Domain.ClaimantInformation;
using DbAddress = AcademyResidentInformationApi.V1.Infrastructure.Address;

namespace AcademyResidentInformationApi.V1.Factories
{
    public static class EntityFactory
    {
        public static ClaimantInformation ToDomain(this Person databaseEntity)
        {
            return new ClaimantInformation
            {
                ClaimId = databaseEntity.ClaimId.Value,
                CheckDigit = databaseEntity.Claim?.CheckDigit,
                PersonRef = databaseEntity.PersonRef.Value,
                FirstName = databaseEntity.FirstName,
                LastName = databaseEntity.LastName,
                NINumber = databaseEntity.NINumber,
                DateOfBirth = databaseEntity.DateOfBirth,
                ClaimantAddress = databaseEntity.Address?.ToDomain()
            };
        }

        public static List<ClaimantInformation> ToDomain(this IEnumerable<Person> people)
        {
            return people.Select(p => p.ToDomain()).ToList();
        }

        public static Address ToDomain(this DbAddress databaseEntity)
        {
            return new Address
            {
                AddressLine1 = databaseEntity.AddressLine1,
                AddressLine2 = databaseEntity.AddressLine2,
                AddressLine3 = databaseEntity.AddressLine3,
                AddressLine4 = databaseEntity.AddressLine4,
                PostCode = databaseEntity.PostCode
            };
        }
    }
}
