using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO Class for Adding a New Country
    /// </summary>
    public class CountryAddRequest
    {
        public string? CountryName { get; set; }
        public Country ToCountry()
        {
            return new Country { CountryName = CountryName };
        }
    }
}
