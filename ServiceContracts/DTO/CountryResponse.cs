using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO Class that is used as return type for most of CuntriesService Methods.
    /// </summary>
    public class CountryResponse
    {
        public Guid CountryId { get; set; }
        public string? CountryName { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if(obj.GetType() != typeof(CountryResponse)) return false;
            CountryResponse county_to_compare = (CountryResponse)obj;
            return CountryId == county_to_compare.CountryId && 
                CountryName == county_to_compare.CountryName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class CountryExtensions
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse()
            {
                CountryId = country.CountryId,
                CountryName = country.CountryName
            };
        }
    }
}
