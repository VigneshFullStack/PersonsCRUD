using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using System.Threading.Tasks;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        // private field
        private readonly PersonsDbContext _db;

        // constructor
        public CountriesService(PersonsDbContext personsDbContext)
        {
            _db = personsDbContext;
		}

        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {
            // Validation: countryAddRequest parameter can't be null
            if(countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            // Validation: CountryName can't be null
            if(countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            // Validation: CountryName can't be duplicate
            if(await _db.Countries.CountAsync(temp => temp.CountryName == countryAddRequest.CountryName) > 0)
            {
                throw new ArgumentException("Given country name already exists");
            }

            // Convert object from CountryAddRequest to Country type
            Country country = countryAddRequest.ToCountry();

            // generate new countryId
            country.CountryId = Guid.NewGuid();

            // Add country object to the list
            _db.Countries.Add(country);
            await _db.SaveChangesAsync();

            return country.ToCountryResponse();
        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
           return await _db.Countries.Select(country => country.ToCountryResponse()).ToListAsync();
        }

        public async Task<CountryResponse>? GetCountryByCountryId(Guid? countryId)
        {
            if(countryId == null) return null;
            Country? countryResponse_from_list = await _db.Countries.FirstOrDefaultAsync(temp => temp.CountryId == countryId);
            if(countryResponse_from_list == null) return null;
            return countryResponse_from_list.ToCountryResponse();
        }
    }
}