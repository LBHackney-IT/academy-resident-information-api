using System.Collections.Generic;
using System.Linq;
using AcademyResidentInformationApi.V1.Domain;
using AcademyResidentInformationApi.V1.Factories;
using AcademyResidentInformationApi.V1.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Address = AcademyResidentInformationApi.V1.Infrastructure.Address;
using AddressDomain = AcademyResidentInformationApi.V1.Domain.Address;
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

        public List<TaxPayerInformation> GetAllTaxPayers(int cursor, int limit, string firstname = null, string lastname = null, string postcode = null, string address = null)
        {
            var firstNameSearchPattern = GetSearchPattern(firstname);
            var lastNameSearchPattern = GetSearchPattern(lastname);
            var addressSearchPattern = GetSearchPattern(address);
            var postcodeSearchPattern = GetSearchPattern(postcode);

            var taxPayers = (
                from taxPayer in _academyContext.TaxPayers
                join occupation in _academyContext.Occupations on taxPayer.AccountRef equals occupation.AccountRef
                join property in _academyContext.CouncilProperties on occupation.PropertyRef equals property.PropertyRef
                join phoneNumber in _academyContext.PhoneNumbers on taxPayer.AccountRef.ToString() equals phoneNumber.Reference
                where string.IsNullOrEmpty(firstname) || EF.Functions.ILike(taxPayer.FirstName, firstNameSearchPattern)
                where string.IsNullOrEmpty(lastname) || EF.Functions.ILike(taxPayer.LastName, lastNameSearchPattern)
                where string.IsNullOrEmpty(address) || EF.Functions.ILike(property.AddressLine1.Replace(" ", ""), addressSearchPattern)
                where string.IsNullOrEmpty(postcode) || EF.Functions.ILike(property.PostCode.Replace(" ", ""), postcodeSearchPattern)
                where taxPayer.AccountRef > cursor
                orderby taxPayer.AccountRef
                select new TaxPayerInformation
                {
                    AccountRef = taxPayer.AccountRef,
                    Uprn = property.Uprn,
                    FirstName = taxPayer.FirstName,
                    LastName = taxPayer.LastName,
                    TaxPayerAddress = new AddressDomain
                    {
                        AddressLine1 = property.AddressLine1,
                        AddressLine2 = property.AddressLine2,
                        AddressLine3 = property.AddressLine3,
                        AddressLine4 = property.AddressLine4,
                        PostCode = property.PostCode
                    },
                    PhoneNumberList = new List<string>
                        {
                            phoneNumber.Number1,
                            phoneNumber.Number2,
                            phoneNumber.Number3,
                            phoneNumber.Number4
                        }
                }
            ).Take(limit).ToList();

            var emailList = _academyContext.Emails
                .Where(email => taxPayers.Select(t => t.AccountRef)
                .Contains(email.ReferenceId))
                .ToList();

            foreach (TaxPayerInformation taxPayer in taxPayers)
            {
                taxPayer.EmailList = emailList
                .Where(email => email.ReferenceId.Equals(taxPayer.AccountRef))
                .Select(email => email.EmailAddress)
                .ToList();
            }

            return taxPayers;
        }

        public TaxPayerInformation GetTaxPayerById(int accountRef)
        {
            var taxPayerRecord = _academyContext.TaxPayers.FirstOrDefault(tp => tp.AccountRef == accountRef);

            if (taxPayerRecord == null) return null;

            var occupationDetails = _academyContext.Occupations.FirstOrDefault(cto => cto.AccountRef.Equals(taxPayerRecord.AccountRef));

            var propertyDetails = _academyContext.CouncilProperties.FirstOrDefault(cp => cp.PropertyRef == occupationDetails.PropertyRef);

            var emailDetails = _academyContext.Emails
                .Where(email => email.ReferenceId.Equals(accountRef))
                .GroupBy(x => x.EmailAddress)
                .Select(x => x.Key)
                .ToList();
            var phoneDetails = _academyContext.PhoneNumbers.FirstOrDefault(phone => phone.Reference == accountRef.ToString());


            return MapDetailsToTaxPayerInformation(taxPayerRecord, propertyDetails, emailDetails, phoneDetails);
        }

        private static ClaimantInformation MapPersonAndAddressesToClaimantInformation(Person person, Address address)
        {
            var claimant = person.ToDomain();
            claimant.ClaimantAddress = address.ToDomain();
            return claimant;
        }

        private static TaxPayerInformation MapDetailsToTaxPayerInformation(TaxPayer taxPayer, CouncilProperty propertyInfo, List<string> emails, PhoneNumber phoneNumbers)
        {
            var person = taxPayer.ToDomain();
            person.Uprn = propertyInfo?.Uprn;
            person.TaxPayerAddress = propertyInfo?.ToDomain();
            person.EmailList = emails;
            person.PhoneNumberList = new List<string> { phoneNumbers?.Number1, phoneNumbers?.Number2, phoneNumbers?.Number3, phoneNumbers?.Number4 };

            person.PhoneNumberList.RemoveAll(item => item == null);

            return person;
        }

        private static string GetSearchPattern(string str)
        {
            return $"%{str?.Replace(" ", "")}%";
        }
    }
}
