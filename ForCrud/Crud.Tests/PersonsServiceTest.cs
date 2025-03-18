using AutoFixture;
using Entities;
using Entities.Data;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTOs;
using ServiceContracts.Enums;
using Services;
using Xunit.Abstractions;

namespace Crud.Tests;

public class PersonsServiceTest
{
    private readonly IPersonsService _personService;
    private readonly ICountriesService _countriesService;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly IFixture _fixture;

    public PersonsServiceTest(ITestOutputHelper testOutputHelper)
    {
        var initialCountries = new List<Country>();
        if (initialCountries == null) throw new ArgumentNullException(nameof(initialCountries));

        var initialPersons = new List<Person>();
        if (initialPersons == null) throw new ArgumentNullException(nameof(initialPersons));

        var dbContextMock =
            new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);

        var dbContext = dbContextMock.Object;
        dbContextMock.CreateDbSetMock(x => x.Countries, initialCountries);
        dbContextMock.CreateDbSetMock(x => x.Persons, initialPersons);

        _countriesService = new CountriesService(dbContext);
        _personService = new PersonsService(dbContext, _countriesService);
        _testOutputHelper = testOutputHelper;
        _fixture = new Fixture();
    }

    #region AddPerson

    //When we supply null value as PersonAddRequest, it should throw ArgumentNullException
    [Fact]
    public async Task AddPerson_NullPerson()
    {
        //Arrange
        PersonAddRequest? personAddRequest = null;

        //Act
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await _personService.AddPersonAsync(personAddRequest);
        });
    }


    //When we supply null value as PersonName, it should throw ArgumentException
    [Fact]
    public async Task AddPerson_PersonNameIsNull()
    {
        //Arrange
        var personAddRequest = _fixture.Build<PersonAddRequest>()
            .With(x => x.PersonName, null as string)
            .Create();

        //Act
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _personService.AddPersonAsync(personAddRequest);
        });
    }

    //When we supply proper person details, it should insert the person into the persons list; and it should return an object of PersonResponse, which includes with the newly generated person id
    [Fact]
    public async Task AddPerson_ProperPersonDetails()
    {
        //Arrange
        var personAddRequest = _fixture.Build<PersonAddRequest>()
            .With(x => x.Email, "test@test.com")
            .Create();

        //Act
        var personResponseFromAdd = await _personService.AddPersonAsync(personAddRequest);

        var personsList = await _personService.GetAllPersonsAsync();

        //Assert
        Assert.True(personResponseFromAdd.PersonId != Guid.Empty);

        Assert.Contains(personResponseFromAdd, personsList);
    }

    #endregion


    #region GetPersonByPersonId

    //If we supply null as PersonId, it should return null as PersonResponse
    [Fact]
    public async Task GetPersonByPersonId_NullPersonId()
    {
        //Arrange
        Guid? personId = null;

        //Act
        var personResponseFromGet = await _personService.GetPersonByPersonIdAsync(personId);

        //Assert
        Assert.Null(personResponseFromGet);
    }


    //If we supply a valid person id, it should return the valid person details as PersonResponse object
    [Fact]
    public async Task GetPersonByPersonId_WithPersonId()
    {
        //Arange
        var countryRequest = _fixture.Create<CountryAddRequest>();
        var countryResponse = await _countriesService.AddCountryAsync(countryRequest);

        PersonAddRequest personRequest = _fixture.Build<PersonAddRequest>()
            .With(x => x.Email, "test@test.com")
            .Create();

        var personResponseFromAdd = await _personService.AddPersonAsync(personRequest);

        var personResponseFromGet =
            await _personService.GetPersonByPersonIdAsync(personResponseFromAdd.PersonId);

        //Assert
        Assert.Equal(personResponseFromAdd, personResponseFromGet);
    }

    #endregion


    #region GetAllPersons

    //The GetAllPersons() should return an empty list by default
    [Fact]
    public async Task GetAllPersons_EmptyList()
    {
        //Act
        var personsFromGet = await _personService.GetAllPersonsAsync();

        //Assert
        Assert.Empty(personsFromGet);
    }


    //First, we will add few persons; and then when we call GetAllPersons(), it should return the same persons that were added
    [Fact]
    public async Task GetAllPersons_AddFewPersons()
    {
        //Arrange
        CountryAddRequest countryRequest1 = _fixture.Create<CountryAddRequest>();
        CountryAddRequest countryRequest2 = _fixture.Create<CountryAddRequest>();

        CountryResponse? countryResponse1 = await _countriesService.AddCountryAsync(countryRequest1);
        CountryResponse? countryResponse2 = await _countriesService.AddCountryAsync(countryRequest2);

        PersonAddRequest personRequest1 = _fixture.Build<PersonAddRequest>()
            .With(x => x.Email, "test@test.com")
            .Create();

        PersonAddRequest personRequest2 = _fixture.Build<PersonAddRequest>()
            .With(x => x.Email, "test@test.com")
            .Create();

        PersonAddRequest personRequest3 = _fixture.Build<PersonAddRequest>()
            .With(x => x.Email, "test@test.com")
            .Create();

        List<PersonAddRequest> personRequests = new List<PersonAddRequest>()
            { personRequest1, personRequest2, personRequest3 };

        List<PersonResponse> personResponseListFromAdd = new List<PersonResponse>();

        foreach (PersonAddRequest personRequest in personRequests)
        {
            PersonResponse personResponse = await _personService.AddPersonAsync(personRequest);
            personResponseListFromAdd.Add(personResponse);
        }

        //print person_response_list_from_add
        _testOutputHelper.WriteLine("Expected:");
        foreach (PersonResponse personResponseFromAdd in personResponseListFromAdd)
        {
            _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
        }

        //Act
        var personsListFromGet = _personService.GetAllPersonsAsync();

        //print persons_list_from_get
        _testOutputHelper.WriteLine("Actual:");
        foreach (PersonResponse personResponseFromGet in await personsListFromGet)
        {
            _testOutputHelper.WriteLine(personResponseFromGet.ToString());
        }

        //Assert
        foreach (PersonResponse personResponseFromAdd in personResponseListFromAdd)
        {
            Assert.Contains(personResponseFromAdd, await personsListFromGet);
        }
    }

    #endregion


    #region GetFilteredPersons

    //If the search text is empty and search by is "PersonName", it should return all persons
    [Fact]
    public async Task GetFilteredPersons_EmptySearchText()
    {
        //Arrange
        CountryAddRequest countryRequest1 = _fixture.Create<CountryAddRequest>();
        CountryAddRequest countryRequest2 = _fixture.Create<CountryAddRequest>();

        CountryResponse? countryResponse1 = await _countriesService.AddCountryAsync(countryRequest1);
        CountryResponse? countryResponse2 = await _countriesService.AddCountryAsync(countryRequest2);

        PersonAddRequest personRequest1 = _fixture.Build<PersonAddRequest>()
            .With(x => x.Email, "test@test.com")
            .Create();

        PersonAddRequest personRequest2 = _fixture.Build<PersonAddRequest>()
            .With(x => x.Email, "test@test.com")
            .Create();

        PersonAddRequest personRequest3 = _fixture.Build<PersonAddRequest>()
            .With(x => x.Email, "test@test.com")
            .Create();

        var personRequests = new List<PersonAddRequest>
            { personRequest1, personRequest2, personRequest3 };
        var personResponseListFromAdd = new List<PersonResponse>();

        foreach (var personRequest in personRequests)
        {
            var personResponse = await _personService.AddPersonAsync(personRequest);
            personResponseListFromAdd.Add(personResponse);
        }

        //print person_response_list_from_add
        _testOutputHelper.WriteLine("Expected:");
        foreach (PersonResponse personResponseFromAdd in personResponseListFromAdd)
        {
            _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
        }

        //Act
        var personsListFromSearch =
            _personService.GetFilteredPersonsAsync(nameof(Person.PersonName), "");

        //print persons_list_from_get
        _testOutputHelper.WriteLine("Actual:");
        foreach (PersonResponse personResponseFromGet in await personsListFromSearch)
        {
            _testOutputHelper.WriteLine(personResponseFromGet.ToString());
        }

        //Assert
        foreach (PersonResponse personResponseFromAdd in personResponseListFromAdd)
        {
            Assert.Contains(personResponseFromAdd, await personsListFromSearch);
        }
    }


    [Fact]
    public async Task GetFilteredPersons_SearchByPersonName()
    {
        //Arrange
        CountryAddRequest countryRequest1 = _fixture.Create<CountryAddRequest>();
        CountryAddRequest countryRequest2 = _fixture.Create<CountryAddRequest>();

        CountryResponse? countryResponse1 = await _countriesService.AddCountryAsync(countryRequest1);
        CountryResponse? countryResponse2 = await _countriesService.AddCountryAsync(countryRequest2);

        PersonAddRequest personRequest1 = _fixture.Build<PersonAddRequest>()
            .With(x => x.Email, "test@test.com")
            .Create();

        PersonAddRequest personRequest2 = _fixture.Build<PersonAddRequest>()
            .With(x => x.Email, "test@test.com")
            .Create();

        PersonAddRequest personRequest3 = _fixture.Build<PersonAddRequest>()
            .With(x => x.Email, "test@test.com")
            .Create();

        List<PersonAddRequest> personRequests = new List<PersonAddRequest>()
            { personRequest1, personRequest2, personRequest3 };

        List<PersonResponse> personResponseListFromAdd = new List<PersonResponse>();

        foreach (PersonAddRequest personRequest in personRequests)
        {
            PersonResponse personResponse = await _personService.AddPersonAsync(personRequest);
            personResponseListFromAdd.Add(personResponse);
        }

        //print person_response_list_from_add
        _testOutputHelper.WriteLine("Expected:");
        foreach (PersonResponse personResponseFromAdd in personResponseListFromAdd)
        {
            _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
        }

        //Act
        var personsListFromSearch =
            await _personService.GetFilteredPersonsAsync(nameof(Person.PersonName), "ma");

        //print persons_list_from_get
        _testOutputHelper.WriteLine("Actual:");
        foreach (PersonResponse personResponseFromGet in personsListFromSearch)
        {
            _testOutputHelper.WriteLine(personResponseFromGet.ToString());
        }

        //Assert
        foreach (var personResponseFromAdd in personResponseListFromAdd)
        {
            if (personResponseFromAdd.PersonName != null)
            {
                if (personResponseFromAdd.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
                {
                    Assert.Contains(personResponseFromAdd, personsListFromSearch);
                }
            }
        }
    }

    #endregion


    #region GetSortedPersons

    //When we sort based on PersonName in DESC, it should return persons list in descending on PersonName
    [Fact]
    public async Task GetSortedPersons()
    {
        //Arrange
        CountryAddRequest countryRequest1 = new CountryAddRequest() { CountryName = "USA" };
        CountryAddRequest countryRequest2 = new CountryAddRequest() { CountryName = "India" };

        CountryResponse? countryResponse1 = await _countriesService.AddCountryAsync(countryRequest1);
        CountryResponse? countryResponse2 = await _countriesService.AddCountryAsync(countryRequest2);

        PersonAddRequest personRequest1 = _fixture.Build<PersonAddRequest>()
            .With(x => x.Email, "test@test.com")
            .Create();

        PersonAddRequest personRequest2 = _fixture.Build<PersonAddRequest>()
            .With(x => x.Email, "test@test.com")
            .Create();

        PersonAddRequest personRequest3 = _fixture.Build<PersonAddRequest>()
            .With(x => x.Email, "test@test.com")
            .Create();

        List<PersonAddRequest> personRequests = new List<PersonAddRequest>()
            { personRequest1, personRequest2, personRequest3 };

        List<PersonResponse> personResponseListFromAdd = new List<PersonResponse>();

        foreach (PersonAddRequest personRequest in personRequests)
        {
            PersonResponse personResponse = await _personService.AddPersonAsync(personRequest);
            personResponseListFromAdd.Add(personResponse);
        }

        //print person_response_list_from_add
        _testOutputHelper.WriteLine("Expected:");
        foreach (PersonResponse personResponseFromAdd in personResponseListFromAdd)
        {
            _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
        }

        var allPersons = await _personService.GetAllPersonsAsync();

        //Act
        var personsListFromSort =
            _personService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.Desc);

        //print persons_list_from_get
        _testOutputHelper.WriteLine("Actual:");
        foreach (PersonResponse personResponseFromGet in personsListFromSort)
        {
            _testOutputHelper.WriteLine(personResponseFromGet.ToString());
        }

        personResponseListFromAdd =
            personResponseListFromAdd.OrderByDescending(temp => temp.PersonName).ToList();

        //Assert
        for (int i = 0; i < personResponseListFromAdd.Count; i++)
        {
            Assert.Equal(personResponseListFromAdd[i], personsListFromSort[i]);
        }
    }

    #endregion


    #region UpdatePerson

    //When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
    [Fact]
    public async Task UpdatePerson_NullPerson()
    {
        //Arrange
        PersonUpdateRequest? personUpdateRequest = null;

        //Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            //Act
            await _personService.UpdatePersonAsync(personUpdateRequest);
        });
    }


    //When we supply invalid person id, it should throw ArgumentException
    [Fact]
    public async Task UpdatePerson_InvalidPersonId()
    {
        //Arrange
        PersonUpdateRequest? personUpdateRequest = _fixture.Create<PersonUpdateRequest>();

        //Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            //Act
            await _personService.UpdatePersonAsync(personUpdateRequest);
        });
    }


    //When PersonName is null, it should throw ArgumentException
    [Fact]
    public async Task UpdatePerson_PersonNameIsNull()
    {
        //Arrange
        CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();
        CountryResponse? countryResponseFromAdd = await _countriesService.AddCountryAsync(countryAddRequest);

        PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>()
            .With(x => x.Email, "test@test.com")
            .Create();

        PersonResponse personResponseFromAdd = await _personService.AddPersonAsync(personAddRequest);

        PersonUpdateRequest personUpdateRequest = personResponseFromAdd.ToPersonUpdateRequest();
        personUpdateRequest.PersonName = null;


        //Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            //Act
            await _personService.UpdatePersonAsync(personUpdateRequest);
        });
    }


    //First, add a new person and try to update the person name and email
    [Fact]
    public async Task UpdatePerson_PersonFullDetailsUpdatetion()
    {
        //Arrange
        CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();
        CountryResponse countryResponseFromAdd = await _countriesService.AddCountryAsync(countryAddRequest);

        PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>()
            .With(x => x.Email, "test@test.com")
            .Create();

        PersonResponse personResponseFromAdd = await _personService.AddPersonAsync(personAddRequest);

        PersonUpdateRequest personUpdateRequest = personResponseFromAdd.ToPersonUpdateRequest();
        personUpdateRequest.PersonName = "William";
        personUpdateRequest.Email = "william@example.com";

        //Act
        PersonResponse personResponseFromUpdate = await _personService.UpdatePersonAsync(personUpdateRequest);

        PersonResponse? personResponseFromGet = await
            _personService.GetPersonByPersonIdAsync(personResponseFromUpdate.PersonId);

        //Assert
        Assert.Equal(personResponseFromGet, personResponseFromUpdate);
    }

    #endregion


    #region DeletePerson

    //If you supply an valid PersonId, it should return true
    [Fact]
    public async Task DeletePerson_ValidPersonId()
    {
        //Arrange
        CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();
        CountryResponse? countryResponseFromAdd = await _countriesService.AddCountryAsync(countryAddRequest);

        PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>()
            .With(x => x.Email, "test@test.com")
            .Create();

        PersonResponse personResponseFromAdd = await _personService.AddPersonAsync(personAddRequest);


        //Act
        bool isDeleted = await _personService.DeletePersonAsync(personResponseFromAdd.PersonId);

        //Assert
        Assert.True(isDeleted);
    }


    //If you supply an invalid PersonId, it should return false
    [Fact]
    public async Task DeletePerson_InvalidPersonId()
    {
        //Act
        var isDeleted = await _personService.DeletePersonAsync(Guid.NewGuid());

        //Assert
        Assert.False(isDeleted);
    }

    #endregion
}