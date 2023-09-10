using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Xunit.Abstractions;
using System.Threading.Tasks;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        // private fields
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;

        // constructor
        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
			_countriesService = new CountriesService(new PersonsDbContext(
                new DbContextOptionsBuilder<PersonsDbContext>().Options));
			_personsService = new PersonsService(new PersonsDbContext(
                new DbContextOptionsBuilder<PersonsDbContext>().Options), _countriesService);
            _testOutputHelper = testOutputHelper;
        }

        #region AddPerson

        // When we supply null value as PersonAddRequest, it should throw ArgumentNullException
        [Fact]
        public void AddPerson_NullPerson()
        {
            // Arrange
            PersonAddRequest? personAddRequest = null;

            // Act
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _personsService.AddPerson(personAddRequest);
            });
        }


        // When we supply null value as PersonName, it should throw ArgumentException
        [Fact]
        public void AddPerson_PersonNameIsNull()
        {
            // Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = null
            };

            // Act
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _personsService.AddPerson(personAddRequest);
            });
        }

        // When we supply proper person details, it should insert the person into the persons list
        // it should return an object of PersonResponse, which includes newly created PersonId
        [Fact]
        public async void AddPerson_ProperPersonDetails()
        {
            // Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = "vignesh",
                Email = "vignesh@gmail.com",
                Address = "thiruvanmiyur",
                CountryId = Guid.NewGuid(),
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-11-17"),
                ReceiveNewsLetters = true
            };

            // Act
            PersonResponse personResponse_from_add_person = await _personsService.AddPerson(personAddRequest);
            List<PersonResponse> person_list = await _personsService.GetAllPersons(); 

            // Assert
            Assert.True(personResponse_from_add_person.PersonId !=  Guid.Empty);
            Assert.Contains(personResponse_from_add_person, person_list);
        }

        #endregion

        #region GetPersonByPersonId

        // If we supply null as PersonId, it should return null as PersonResponse
        [Fact]
        public async void GetPersonByPersonId_NullPersonId()
        {
            // Arrange
            Guid? personId = null;

            // Act
            PersonResponse? personResponse_from_get = await _personsService.GetPersonByPersonId(personId);

            // Assert
            Assert.Null(personResponse_from_get);
        }

        // If we supply a valid PersonId, it should return the valid person details as PersonResponse object
        [Fact]
        public async void GetPersonByPersonId_WithPersonId()
        {
            // Arrange
            CountryAddRequest country_request = new CountryAddRequest()
            {
                CountryName = "INDIA"
            };
            CountryResponse country_response = await _countriesService.AddCountry(country_request);

            // Act
            PersonAddRequest person_request = new PersonAddRequest()
            {
                PersonName = "vignesh",
                Email = "vignesh@gmail.com",
                Address = "thiruvanmiyur",
                CountryId = country_response.CountryId,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-11-17"),
                ReceiveNewsLetters = false
            };
            PersonResponse person_response_from_add = await _personsService.AddPerson(person_request);
            PersonResponse? person_response_from_get = await _personsService.GetPersonByPersonId(person_response_from_add.PersonId);

            // Assert
            Assert.Equal(person_response_from_add, person_response_from_get);
        }

        #endregion

        #region GetAllPersons

        // The GetAllPersons() should return an empty list by default
        [Fact]
        public async void GetAllPersons_EmptyList()
        {
            // Act
            List<PersonResponse> person_from_get = await _personsService.GetAllPersons();

            // Assert
            Assert.Empty(person_from_get);
        }

        // First, we will add few persons, and then when we call GetAllPersons(), 
        // it should return the same persons that were added
        [Fact]
        public async void GetAllPersons_AddFewPersons()
        {
            // Arrange
            CountryAddRequest countryAddRequest1 = new CountryAddRequest() { CountryName = "INDIA" };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest() { CountryName = "UK" };

            CountryResponse countryResponse1 = await _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(countryAddRequest2);

            PersonAddRequest personAddRequest1 = new PersonAddRequest()
            {
                PersonName = "vignesh",
                Email = "vignesh@gmail.com",
                Address = "address of vignesh",
                CountryId = countryResponse1.CountryId,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-11-17"),
                ReceiveNewsLetters = true
            };

            PersonAddRequest personAddRequest2 = new PersonAddRequest()
            {
                PersonName = "monisha",
                Email = "monisha@gmail.com",
                Address = "address of monisha",
                CountryId = countryResponse1.CountryId,
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("1997-05-31"),
                ReceiveNewsLetters = true
            };

            PersonAddRequest personAddRequest3 = new PersonAddRequest()
            {
                PersonName = "bhuvi",
                Email = "bhuvi@gmail.com",
                Address = "address of bhuvi",
                CountryId = countryResponse2.CountryId,
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("1998-03-29"),
                ReceiveNewsLetters = true
            };

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                personAddRequest1,
                personAddRequest2,
                personAddRequest3
            };

            List<PersonResponse> person_list_response_from_add = new List<PersonResponse>();
            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _personsService.AddPerson(person_request);
                person_list_response_from_add.Add(person_response);
            }

            // print person_list_response_from_add
            _testOutputHelper.WriteLine("Expected : ");
            foreach (PersonResponse person_response_from_add in person_list_response_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            // Act
            List<PersonResponse> person_list_from_get = await _personsService.GetAllPersons();

            // print person_list_response_from_add
            _testOutputHelper.WriteLine("Actual : ");
            foreach (PersonResponse person_response_from_get in person_list_from_get)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            // Assert
            foreach (PersonResponse person_response_from_add in person_list_response_from_add)
            {
                Assert.Contains(person_response_from_add, person_list_from_get);
            }
        }

        #endregion

        #region GetFilteredPersons

        // If the search text is empty and search by is PersonName, it should return all persons
        [Fact]
        public async void GetFilteredPersons_EmptySearchText()
        {
            // Arrange
            CountryAddRequest countryAddRequest1 = new CountryAddRequest() { CountryName = "INDIA" };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest() { CountryName = "UK" };

            CountryResponse countryResponse1 = await _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(countryAddRequest2);

            PersonAddRequest personAddRequest1 = new PersonAddRequest()
            {
                PersonName = "vignesh",
                Email = "vignesh@gmail.com",
                Address = "address of vignesh",
                CountryId = countryResponse1.CountryId,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-11-17"),
                ReceiveNewsLetters = true
            };

            PersonAddRequest personAddRequest2 = new PersonAddRequest()
            {
                PersonName = "monisha",
                Email = "monisha@gmail.com",
                Address = "address of monisha",
                CountryId = countryResponse1.CountryId,
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("1997-05-31"),
                ReceiveNewsLetters = true
            };

            PersonAddRequest personAddRequest3 = new PersonAddRequest()
            {
                PersonName = "bhuvi",
                Email = "bhuvi@gmail.com",
                Address = "address of bhuvi",
                CountryId = countryResponse2.CountryId,
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("1998-03-29"),
                ReceiveNewsLetters = true
            };

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                personAddRequest1,
                personAddRequest2,
                personAddRequest3
            };

            List<PersonResponse> person_list_response_from_add = new List<PersonResponse>();
            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _personsService.AddPerson(person_request);
                person_list_response_from_add.Add(person_response);
            }

            // print person_list_response_from_add
            _testOutputHelper.WriteLine("Expected : ");
            foreach (PersonResponse person_response_from_add in person_list_response_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            // Act
            List<PersonResponse> person_list_from_search = await _personsService.GetFilteredPersons(nameof(Person.PersonName), "");

            // print person_list_response_from_add
            _testOutputHelper.WriteLine("Actual : ");
            foreach (PersonResponse person_response_from_get in person_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            // Assert
            foreach (PersonResponse person_response_from_add in person_list_response_from_add)
            {
                Assert.Contains(person_response_from_add, person_list_from_search);
            }
        }

        // First we will add few persons; and then we will search based on person name with some search string.
        // It should returns the matching persons.
        [Fact]
        public async void GetFilteredPersons_SearchByPersonName()
        {
            // Arrange
            CountryAddRequest countryAddRequest1 = new CountryAddRequest() { CountryName = "INDIA" };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest() { CountryName = "UK" };

            CountryResponse countryResponse1 = await _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(countryAddRequest2);

            PersonAddRequest personAddRequest1 = new PersonAddRequest()
            {
                PersonName = "vignesh",
                Email = "vignesh@gmail.com",
                Address = "address of vignesh",
                CountryId = countryResponse1.CountryId,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-11-17"),
                ReceiveNewsLetters = true
            };

            PersonAddRequest personAddRequest2 = new PersonAddRequest()
            {
                PersonName = "monisha",
                Email = "monisha@gmail.com",
                Address = "address of monisha",
                CountryId = countryResponse1.CountryId,
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("1997-05-31"),
                ReceiveNewsLetters = true
            };

            PersonAddRequest personAddRequest3 = new PersonAddRequest()
            {
                PersonName = "bhuvi",
                Email = "bhuvi@gmail.com",
                Address = "address of bhuvi",
                CountryId = countryResponse2.CountryId,
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("1998-03-29"),
                ReceiveNewsLetters = true
            };

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                personAddRequest1,
                personAddRequest2,
                personAddRequest3
            };

            List<PersonResponse> person_list_response_from_add = new List<PersonResponse>();
            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _personsService.AddPerson(person_request);
                person_list_response_from_add.Add(person_response);
            }

            // print person_list_response_from_add
            _testOutputHelper.WriteLine("Expected : ");
            foreach (PersonResponse person_response_from_add in person_list_response_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            // Act
            List<PersonResponse> person_list_from_search = await _personsService.GetFilteredPersons(nameof(Person.PersonName), "moni");

            // print person_list_response_from_add
            _testOutputHelper.WriteLine("Actual : ");
            foreach (PersonResponse person_response_from_get in person_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            // Assert
            foreach (PersonResponse person_response_from_add in person_list_response_from_add)
            {
               if(person_response_from_add.PersonName != null)
                {
                    if (person_response_from_add.PersonName.Contains("moni", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(person_response_from_add, person_list_from_search);
                    }
                }
            }
        }

        #endregion

        #region GetSortedPersons

        // When we sort based on PersonName in DESC, it should return persons list in descending order
        [Fact]
        public async void GetSortedPersons()
        {
            // Arrange
            CountryAddRequest countryAddRequest1 = new CountryAddRequest() { CountryName = "INDIA" };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest() { CountryName = "UK" };

            CountryResponse countryResponse1 = await _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(countryAddRequest2);

            PersonAddRequest personAddRequest1 = new PersonAddRequest()
            {
                PersonName = "vignesh",
                Email = "vignesh@gmail.com",
                Address = "address of vignesh",
                CountryId = countryResponse1.CountryId,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-11-17"),
                ReceiveNewsLetters = true
            };

            PersonAddRequest personAddRequest2 = new PersonAddRequest()
            {
                PersonName = "monisha",
                Email = "monisha@gmail.com",
                Address = "address of monisha",
                CountryId = countryResponse1.CountryId,
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("1997-05-31"),
                ReceiveNewsLetters = true
            };

            PersonAddRequest personAddRequest3 = new PersonAddRequest()
            {
                PersonName = "bhuvi",
                Email = "bhuvi@gmail.com",
                Address = "address of bhuvi",
                CountryId = countryResponse2.CountryId,
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("1998-03-29"),
                ReceiveNewsLetters = true
            };

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                personAddRequest1,
                personAddRequest2,
                personAddRequest3
            };

            List<PersonResponse> person_list_response_from_add = new List<PersonResponse>();
            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _personsService.AddPerson(person_request);
                person_list_response_from_add.Add(person_response);
            }

            // print person_list_response_from_add
            _testOutputHelper.WriteLine("Expected : ");
            foreach (PersonResponse person_response_from_add in person_list_response_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            List<PersonResponse> allPersons = await _personsService.GetAllPersons();

            // Act
            List<PersonResponse> person_list_from_sort = await _personsService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

            // print person_list_response_from_add
            _testOutputHelper.WriteLine("Actual : ");
            foreach (PersonResponse person_response_from_get in person_list_from_sort)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            person_list_response_from_add = person_list_response_from_add.OrderByDescending(temp => temp.PersonName).ToList();

            // Assert
            for (int i = 0; i < person_list_response_from_add.Count; i++)
            {
                Assert.Equal(person_list_response_from_add[i], person_list_from_sort[i]);
            }
        }

        #endregion

        #region UpdatePerson

        // When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
        [Fact]
        public void UpdatePerson_NullPerson()
        {
            // Arrange 
            PersonUpdateRequest? personUpdateRequest = null;

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await _personsService.UpdatePerson(personUpdateRequest);
            });
        }

        // When we supply invalid PersonId, it should throw ArgumentException
        [Fact]
        public void UpdatePerson_InvalidPersonId()
        {
            // Arrange 
            PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest()
            {
                PersonId = Guid.NewGuid()
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act
               await _personsService.UpdatePerson(personUpdateRequest);
            });
        }

        // When PersonName is null, it should throw ArgumentException
        [Fact]
        public async void UpdatePerson_PersonNameIsNull()
        {
            // Arrange 
            CountryAddRequest country_add_request = new CountryAddRequest()
            {
                CountryName = "INDIA"
            };
            CountryResponse country_response_from_add = await _countriesService.AddCountry(country_add_request);

            PersonAddRequest person_add_request = new PersonAddRequest()
            {
                PersonName = "monisha",
                Email = "monisha@gmail.com",
                Address = "address of monisha",
                CountryId = country_response_from_add.CountryId,
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("1997-05-31"),
                ReceiveNewsLetters = true
            };
            PersonResponse person_response_from_add = await _personsService.AddPerson(person_add_request);

            PersonUpdateRequest? person_update_request = person_response_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = null;

            // Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act
                await _personsService.UpdatePerson(person_update_request);
            });
        }

        // First, add a new person and try to update the person name and email
        [Fact]
        public async void UpdatePerson_PersonDetailsUpdation()
        {
            // Arrange 
            CountryAddRequest country_add_request = new CountryAddRequest()
            {
                CountryName = "INDIA"
            };
            CountryResponse country_response_from_add = await _countriesService.AddCountry(country_add_request);

            PersonAddRequest person_add_request = new PersonAddRequest()
            {
                PersonName = "monisha",
                Email = "monisha@gmail.com",
                Address = "address of monisha",
                CountryId = country_response_from_add.CountryId,
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("1997-05-31"),
                ReceiveNewsLetters = true
            };
            PersonResponse person_response_from_add = await _personsService.AddPerson(person_add_request);

            PersonUpdateRequest? person_update_request = person_response_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = "Moni";
            person_update_request.Email = "moni@gmail.com";

            // Act
            PersonResponse person_response_from_update = await _personsService.UpdatePerson(person_update_request);

            PersonResponse? person_response_from_get = await _personsService.GetPersonByPersonId(person_response_from_update.PersonId);

            // Assert
            Assert.Equal(person_response_from_get, person_response_from_update);
        }

        #endregion

        #region DeletePerson

        // If you supply an valid PersonId, it should returns true
        [Fact]
        public async void DeletePerson_ValidPersonId()
        {
            // Arrange
            CountryAddRequest country_add_request = new CountryAddRequest()
            {
                CountryName = "INDIA"
            };
            CountryResponse country_response_from_add = await _countriesService.AddCountry(country_add_request);

            PersonAddRequest person_add_request = new PersonAddRequest()
            {
                PersonName = "monisha",
                Email = "monisha@gmail.com",
                Address = "address of monisha",
                CountryId = country_response_from_add.CountryId,
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("1997-05-31"),
                ReceiveNewsLetters = true
            };
            PersonResponse person_response_from_add = await _personsService.AddPerson(person_add_request);

            // Act
            bool isDeleted = await _personsService.DeletePerson(person_response_from_add.PersonId);

            // Assert
            Assert.True(isDeleted);
        }

        // If you supply an invalid PersonId, it should returns false
        [Fact]
        public async void DeletePerson_InvalidPersonId()
        {
            // Act
            bool isDeleted = await _personsService.DeletePerson(Guid.NewGuid());

            // Assert
            Assert.False(isDeleted);
        }

        #endregion
    }
}
