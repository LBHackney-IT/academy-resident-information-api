using System;
using System.Collections.Generic;
using System.Linq;
using AcademyResidentInformationApi.V1.Domain;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Address = AcademyResidentInformationApi.V1.Infrastructure.Address;
using ClaimantInformation = AcademyResidentInformationApi.V1.Domain.ClaimantInformation;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Query;

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

            var (firstHalf, secondHalf) = SplitPostcode(postcode);
            var query = (
                from person in _academyContext.Persons
                join a in _academyContext.Addresses on new { person.ClaimId, person.HouseId } equals new { a.ClaimId, a.HouseId }
                join c in _academyContext.Claims on person.ClaimId equals c.ClaimId
                where a.ToDate == "2099-12-31 00:00:00.0000000"
                where person.ClaimId > cursor.ClaimId || person.HouseId > cursor.HouseId || person.MemberId > cursor.MemberId
                where string.IsNullOrEmpty(address) || EF.Functions.ILike(a.AddressLine1.Replace(" ", ""), addressSearchPattern)
                where string.IsNullOrEmpty(postcode)
                      || postcode.Length > 3 &&  EF.Functions.ILike(a.PostCode, firstHalf) && EF.Functions.ILike(a.PostCode, secondHalf)
                      || EF.Functions.ILike(a.PostCode, $"%{postcode}%")
                where string.IsNullOrEmpty(firstname) || EF.Functions.ILike(person.FirstName, firstNameSearchPattern)
                where string.IsNullOrEmpty(lastname) || EF.Functions.ILike(person.LastName, lastNameSearchPattern)
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
                ).Take(limit);
            Console.Write(query.ToSql());
            return query.ToList().ToDomain();
        }

        private static (string, string) SplitPostcode(string postcode)
        {
            if (postcode == null) return (null, null);
            if (postcode.Length <= 3) return (postcode, null);
            var whiteSpaceReplaced = postcode?.Replace(" ", "");
            var firstHalf = $"%{whiteSpaceReplaced.Substring(0, whiteSpaceReplaced.Length - 3)}%";
            var secondHalf = $"%{whiteSpaceReplaced.Substring(whiteSpaceReplaced.Length - 3, 3)}%";
            return (firstHalf, secondHalf);
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

    public static class SqlExtension
    {
        public static string ToSql<TEntity>(this IQueryable<TEntity> query) where TEntity : class
        {
            var enumerator = query.Provider.Execute<IEnumerable<TEntity>>(query.Expression).GetEnumerator();
            var relationalCommandCache = enumerator.Private("_relationalCommandCache");
            var selectExpression = relationalCommandCache.Private<SelectExpression>("_selectExpression");
            var factory = relationalCommandCache.Private<IQuerySqlGeneratorFactory>("_querySqlGeneratorFactory");

            var sqlGenerator = factory.Create();
            var command = sqlGenerator.GetCommand(selectExpression);

            string sql = command.CommandText;
            return sql;
        }

        private static object Private(this object obj, string privateField) => obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);
        private static T Private<T>(this object obj, string privateField) => (T)obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);
    }
}
