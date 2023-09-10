using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System.Threading.Tasks;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        // private fields
        private readonly ICountriesService _countriesService;

        // constructor
        public CountriesServiceTest()
        {
            _countriesService = new CountriesService(
                new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options));
        }

        #region AddCountry
        // When CountryAddRequest is null, it should throw ArgumentNullException
        [Fact]
        public async Task AddCountry_NullCountry()
        {
            // Arrange
            CountryAddRequest? request = null;

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await _countriesService.AddCountry(request);
            });
        }

        // When the CountryName is null, it should throw ArgumentException
        [Fact]
        public async Task AddCountry_CountryNameIsNull()
        {
            // Arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = null
            };

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act
                await _countriesService.AddCountry(request);
            });
        }

        // When the CountryName is Duplicate, it should throw ArgumentException
        [Fact]
        public async Task AddCountry_CountryNameIsDuplicate()
        {
            // Arrange
            CountryAddRequest? request1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryAddRequest? request2 = new CountryAddRequest()
            {
                CountryName = "USA"
            };

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act
                await _countriesService.AddCountry(request1);
                await _countriesService.AddCountry(request2);
            });
        }

        // When you supply proper CountryName, it should insert the country to the existing list
        [Fact]
        public async Task AddCountry_ProperCountryDetails()
        {
            // Arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = "India"
            };

            // Act
            CountryResponse response = await _countriesService.AddCountry(request);
            List<CountryResponse> countries_from_getAllCountries = await _countriesService.GetAllCountries();

            // Assert
            Assert.True(response.CountryId != Guid.Empty);
            Assert.Contains(response, countries_from_getAllCountries);
        }
        #endregion

        #region GetAllCountries

        // The list of countries should be empty by default (before adding any countries)
        [Fact]
        public async Task GetAllCountries_EmptyList()
        {
            // Act
            List<CountryResponse> actual_country_respose_list = await _countriesService.GetAllCountries();

            // Assert
            Assert.Empty(actual_country_respose_list);
        }

        [Fact]
        public async Task GetAllCountries_AddFewCountries()
        {
            // Arrange
            List<CountryAddRequest> county_request_list = new List<CountryAddRequest>()
            {
                new CountryAddRequest() {CountryName = "USA"},
                new CountryAddRequest() {CountryName = "INDIA"},
                new CountryAddRequest() {CountryName = "UK"}
            };

            // Act
            List<CountryResponse> country_list_from_add_country = new List<CountryResponse>();
            foreach (CountryAddRequest country_request in county_request_list)
            {
                country_list_from_add_country.Add(await _countriesService.AddCountry(country_request));
            }

            List<CountryResponse> actual_country_response_list = await _countriesService.GetAllCountries();

            // read each element from country_list_from_add_country
            foreach (CountryResponse expected_country in country_list_from_add_country)
            {
                Assert.Contains(expected_country, actual_country_response_list);
            }
        }
        #endregion

        #region GetCountryByCountryId

        // If we supply null as CountryId, it should return null as CountryResponse
        [Fact]
        public async Task GetCountryByCountyId_NullCountryId()
        {
            // Arrange
            Guid? countryId = null;

            // Act
            CountryResponse? countryResponse = await _countriesService.GetCountryByCountryId(countryId);

            // Assert
            Assert.Null(countryResponse);
        }

        // If we supply a valid CountryId, it should return the matching country details as CountryResponse object
        [Fact]
        public async Task GetCountryByCountryId_ValidCountryId()
        {
            // Arrange 
            CountryAddRequest? countryAddRequest = new CountryAddRequest()
            {
                CountryName = "INDIA"
            };
            CountryResponse countryResponse_from_add = await _countriesService.AddCountry(countryAddRequest);

            // Act
            CountryResponse? countryResponse_from_get = await _countriesService.GetCountryByCountryId(countryResponse_from_add.CountryId);

            // Assert
            Assert.Equal(countryResponse_from_add, countryResponse_from_get);
        }
        #endregion
    }
}
