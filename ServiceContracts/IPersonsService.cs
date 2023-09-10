using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System.Threading.Tasks;

namespace ServiceContracts
{
    /// <summary>
    /// Represents the business logic for Person entity
    /// </summary>
    public interface IPersonsService
    {
        /// <summary>
        /// Adds a new Person into the list of persons
        /// </summary>
        /// <param name="personAddRequest">Person object to add</param>
        /// <returns>Returns the same person details, along with newly created PersonId</returns>
        Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);

        /// <summary>
        /// Returns all Persons
        /// </summary>
        /// <returns>Returns a list of objects of PersonResponse type</returns>
        Task<List<PersonResponse>> GetAllPersons();

        /// <summary>
        /// Returns the Person object based on the given PersonId
        /// </summary>
        /// <param name="personId">person id to search</param>
        /// <returns>Returns matching person Object</returns>
        Task<PersonResponse?> GetPersonByPersonId(Guid? personId);

        /// <summary>
        /// Returns all the person objects that matches with the given search field and search string
        /// </summary>
        /// <param name="searchBy">Search field to search</param>
        /// <param name="searchString">Search string to search</param>
        /// <returns>Returns all person objects based on the given search field and search string</returns>
        Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString);

        /// <summary>
        /// Returns Sorted list of Persons
        /// </summary>
        /// <param name="allPersons">Represents list of persons to sort</param>
        /// <param name="sortBy">Name of the property, based on the persons should be sorted</param>
        /// <param name="sortOrder">ASC or DESC</param>
        /// <returns>Returns sorted persons as PersonResponse list</returns>
        Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons,
            string sortBy, SortOrderOptions sortOrder);

        /// <summary>
        /// Updates the specific person details based on the given PersonId
        /// </summary>
        /// <param name="personUpdateRequest">Person Details to Update</param>
        /// <returns>Returns the Person Object after the Updation</returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        /// <summary>
        /// Deletes a Person based on given PersonId
        /// </summary>
        /// <param name="personId">PersonId to Delete</param>
        /// <returns>Returns true, if the deletion is successful, otherwise false</returns>
        Task<bool> DeletePerson(Guid? personId);


        /// <summary>
        /// Returns persons as CSV
        /// </summary>
        /// <returns>Returns the memory stream with CSV data of persons</returns>
        Task<MemoryStream> GetPersonsCSV();

        /// <summary>
        /// Returns persons as Excel
        /// </summary>
        /// <returns>Returns the memory stream with Excel data of persons</returns>
        Task<MemoryStream> GetPersonsExcel();
    }
}
 