using ServiceContracts.DTO;
using System.Threading.Tasks;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Country entity
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Adds a Country object to the list of Countries
        /// </summary>
        /// <param name="countryAddRequest">Country object to Add</param>
        /// <returns>Returns the Country object after adding it (including newly generated country id)</returns>
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);

        /// <summary>
        /// Returns all the countries from the list
        /// </summary>
        /// <returns>All countries from the list as List of CountryResponse</returns>
        Task<List<CountryResponse>> GetAllCountries();

        /// <summary>
        /// Returns a Country object based on a Country Id
        /// </summary>
        /// <param name="countryId">CountryId (guid) to Search</param>
        /// <returns>Matching Country as CountryResponse object</returns>
        Task<CountryResponse>? GetCountryByCountryId(Guid? countryId);
    }
} 