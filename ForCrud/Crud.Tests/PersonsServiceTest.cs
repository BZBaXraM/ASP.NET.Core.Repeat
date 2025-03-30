using System.Linq.Expressions;
using AutoFixture;
using Crud.Core.Domain.Entities;
using Crud.Core.DTOs;
using Crud.Core.Enums;
using Crud.Core.RepositoryContracts;
using Crud.Core.ServiceContracts;
using Crud.Core.Services;
using Crud.Infrastructure.Data;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit.Abstractions;

namespace Crud.Tests;

public class PersonsServiceTest
{
    //private fields
    private readonly IPersonsService _personService;
    private readonly ICountriesService _countriesService;

    private readonly Mock<IPersonsRepository> _personRepositoryMock;

    private readonly ITestOutputHelper _testOutputHelper;
    private readonly IFixture _fixture;

    //constructor
    public PersonsServiceTest(ITestOutputHelper testOutputHelper)
    {
        _fixture = new Fixture();
        _personRepositoryMock = new Mock<IPersonsRepository>();
        var personsRepository = _personRepositoryMock.Object;

        var countriesInitialData = new List<Country>();
        if (countriesInitialData == null) throw new ArgumentNullException(nameof(countriesInitialData));
        var personsInitialData = new List<Person>();
        if (personsInitialData == null) throw new ArgumentNullException(nameof(personsInitialData));

        // Create mock for DbContext
        var dbContextMock = new DbContextMock<ApplicationDbContext>(
            new DbContextOptionsBuilder<ApplicationDbContext>().Options
        );

        //Access Mock DbContext object
        var dbContext = dbContextMock.Object;

        //Create mocks for DbSets'
        dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);
        dbContextMock.CreateDbSetMock(temp => temp.Persons, personsInitialData);

        //Create services based on mocked DbContext object
        _countriesService = new CountriesService(null);

        _personService = new PersonsService(personsRepository);

        _testOutputHelper = testOutputHelper;
    }

    #region AddPerson

    //When we supply null value as PersonAddRequest, it should throw ArgumentNullException
    [Fact]
    public async Task AddPerson_NullPerson_ToBeArgumentNullException()
    {
        //Arrange
        PersonAddRequest? personAddRequest = null;

        //Act
        var action = async () => { await _personService.AddPersonAsync(personAddRequest); };

        //Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }


    //When we supply null value as PersonName, it should throw ArgumentException
    [Fact]
    public async Task AddPerson_PersonNameIsNull_ToBeArgumentException()
    {
        //Arrange
        var personAddRequest = _fixture.Build<PersonAddRequest>()
            .With(temp => temp.PersonName, null as string)
            .Create();

        var person = personAddRequest.ToPerson();

        //When PersonsRepository.AddPerson is called, it has to return the same "person" object
        _personRepositoryMock
            .Setup(temp => temp.AddPersonAsync(It.IsAny<Person>()))
            .ReturnsAsync(person);

        //Act
        var action = async () => { await _personService.AddPersonAsync(personAddRequest); };

        //Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }


    //When we supply proper person details, it should insert the person into the persons list; and it should return an object of PersonResponse, which includes with the newly generated person id
    [Fact]
    public async Task AddPerson_FullPersonDetails_ToBeSuccessful()
    {
        //Arrange
        PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
            .With(temp => temp.Email, "someone@example.com")
            .Create();

        Person person = personAddRequest.ToPerson();
        PersonResponse personResponseExpected = person.ToPersonResponse();

        //If we supply any argument value to the AddPerson method, it should return the same return value
        _personRepositoryMock.Setup
                (temp => temp.AddPersonAsync(It.IsAny<Person>()))
            .ReturnsAsync(person);

        //Act
        PersonResponse personResponseFromAdd = await _personService.AddPersonAsync(personAddRequest);

        personResponseExpected.PersonId = personResponseFromAdd.PersonId;

        //Assert
        personResponseFromAdd.PersonId.Should().NotBe(Guid.Empty);
        personResponseFromAdd.Should().Be(personResponseExpected);
    }

    #endregion


    #region GetPersonByPersonID

    //If we supply null as PersonID, it should return null as PersonResponse
    [Fact]
    public async Task GetPersonByPersonID_NullPersonID_ToBeNull()
    {
        //Arrange
        Guid? personId = null;

        //Act
        PersonResponse? personResponseFromGet = await _personService.GetPersonByPersonIdAsync(personId);

        //Assert
        personResponseFromGet.Should().BeNull();
    }


    //If we supply a valid person id, it should return the valid person details as PersonResponse object
    [Fact]
    public async Task GetPersonByPersonID_WithPersonID_ToBeSuccessful()
    {
        //Arange
        Person person = _fixture.Build<Person>()
            .With(temp => temp.Email, "email@sample.com")
            .With(temp => temp.Country, null as Country)
            .Create();
        PersonResponse personResponseExpected = person.ToPersonResponse();

        _personRepositoryMock.Setup(temp => temp.GetPersonByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(person);

        //Act
        PersonResponse? personResponseFromGet = await _personService.GetPersonByPersonIdAsync(person.PersonId);

        //Assert
        personResponseFromGet.Should().Be(personResponseExpected);
    }

    #endregion


    #region GetAllPersons

    //The GetAllPersons() should return an empty list by default
    [Fact]
    public async Task GetAllPersons_ToBeEmptyList()
    {
        //Arrange
        var persons = new List<Person>();
        _personRepositoryMock
            .Setup(temp => temp.GetPersonsAsync())
            .ReturnsAsync(persons);

        //Act
        List<PersonResponse> personsFromGet = await _personService.GetAllPersonsAsync();

        //Assert
        personsFromGet.Should().BeEmpty();
    }


    //First, we will add few persons; and then when we call GetAllPersons(), it should return the same persons that were added
    [Fact]
    public async Task GetAllPersons_WithFewPersons_ToBeSuccessful()
    {
        //Arrange
        List<Person> persons = new List<Person>()
        {
            _fixture.Build<Person>()
                .With(temp => temp.Email, "someone_1@example.com")
                .With(temp => temp.Country, null as Country)
                .Create(),

            _fixture.Build<Person>()
                .With(temp => temp.Email, "someone_2@example.com")
                .With(temp => temp.Country, null as Country)
                .Create(),

            _fixture.Build<Person>()
                .With(temp => temp.Email, "someone_3@example.com")
                .With(temp => temp.Country, null as Country)
                .Create()
        };

        List<PersonResponse> personResponseListExpected = persons.Select(temp => temp.ToPersonResponse()).ToList();


        //print person_response_list_from_add
        _testOutputHelper.WriteLine("Expected:");
        foreach (PersonResponse personResponseFromAdd in personResponseListExpected)
        {
            _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
        }

        _personRepositoryMock.Setup(temp => temp.GetPersonsAsync()).ReturnsAsync(persons);

        //Act
        List<PersonResponse> personsListFromGet = await _personService.GetAllPersonsAsync();

        //print persons_list_from_get
        _testOutputHelper.WriteLine("Actual:");
        foreach (PersonResponse personResponseFromGet in personsListFromGet)
        {
            _testOutputHelper.WriteLine(personResponseFromGet.ToString());
        }

        //Assert
        personsListFromGet.Should().BeEquivalentTo(personResponseListExpected);
    }

    #endregion


    #region GetFilteredPersons

    //If the search text is empty and search by is "PersonName", it should return all persons
    [Fact]
    public async Task GetFilteredPersons_EmptySearchText_ToBeSuccessful()
    {
        //Arrange
        List<Person> persons = new List<Person>()
        {
            _fixture.Build<Person>()
                .With(temp => temp.Email, "someone_1@example.com")
                .With(temp => temp.Country, null as Country)
                .Create(),

            _fixture.Build<Person>()
                .With(temp => temp.Email, "someone_2@example.com")
                .With(temp => temp.Country, null as Country)
                .Create(),

            _fixture.Build<Person>()
                .With(temp => temp.Email, "someone_3@example.com")
                .With(temp => temp.Country, null as Country)
                .Create()
        };

        List<PersonResponse> personResponseListExpected = persons.Select(temp => temp.ToPersonResponse()).ToList();


        //print person_response_list_from_add
        _testOutputHelper.WriteLine("Expected:");
        foreach (PersonResponse personResponseFromAdd in personResponseListExpected)
        {
            _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
        }

        _personRepositoryMock.Setup(temp => temp
                .GetFilteredPersonsAsync(It.IsAny<Expression<Func<Person, bool>>>()))
            .ReturnsAsync(persons);

        //Act
        List<PersonResponse> personsListFromSearch =
            await _personService.GetFilteredPersonsAsync(nameof(Person.PersonName), "");

        //print persons_list_from_get
        _testOutputHelper.WriteLine("Actual:");
        foreach (PersonResponse personResponseFromGet in personsListFromSearch)
        {
            _testOutputHelper.WriteLine(personResponseFromGet.ToString());
        }

        //Assert
        personsListFromSearch.Should().BeEquivalentTo(personResponseListExpected);
    }


    //Search based on person name with some search string. It should return the matching persons
    [Fact]
    public async Task GetFilteredPersons_SearchByPersonName_ToBeSuccessful()
    {
        //Arrange
        List<Person> persons =
        [
            _fixture.Build<Person>()
                .With(temp => temp.Email, "someone_1@example.com")
                .With(temp => temp.Country, null as Country)
                .Create(),


            _fixture.Build<Person>()
                .With(temp => temp.Email, "someone_2@example.com")
                .With(temp => temp.Country, null as Country)
                .Create(),


            _fixture.Build<Person>()
                .With(temp => temp.Email, "someone_3@example.com")
                .With(temp => temp.Country, null as Country)
                .Create()
        ];

        var personResponseListExpected = persons.Select(temp => temp.ToPersonResponse()).ToList();


        //print person_response_list_from_add
        _testOutputHelper.WriteLine("Expected:");
        foreach (var personResponseFromAdd in personResponseListExpected)
        {
            _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
        }

        _personRepositoryMock.Setup(temp => temp
                .GetFilteredPersonsAsync(It.IsAny<Expression<Func<Person, bool>>>()))
            .ReturnsAsync(persons);

        //Act
        var personsListFromSearch =
            await _personService.GetFilteredPersonsAsync(nameof(Person.PersonName), "sa");

        //print persons_list_from_get
        _testOutputHelper.WriteLine("Actual:");
        foreach (var personResponseFromGet in personsListFromSearch)
        {
            _testOutputHelper.WriteLine(personResponseFromGet.ToString());
        }

        //Assert
        personsListFromSearch.Should().BeEquivalentTo(personResponseListExpected);
    }

    #endregion


    #region GetSortedPersons

    //When we sort based on PersonName in DESC, it should return persons list in descending on PersonName
    [Fact]
    public async Task GetSortedPersons_ToBeSuccessful()
    {
        //Arrange
        var persons = new List<Person>
        {
            _fixture.Build<Person>()
                .With(temp => temp.Email, "someone_1@example.com")
                .With(temp => temp.Country, null as Country)
                .Create(),

            _fixture.Build<Person>()
                .With(temp => temp.Email, "someone_2@example.com")
                .With(temp => temp.Country, null as Country)
                .Create(),

            _fixture.Build<Person>()
                .With(temp => temp.Email, "someone_3@example.com")
                .With(temp => temp.Country, null as Country)
                .Create()
        };

        var personResponseListExpected = persons.Select(temp => temp.ToPersonResponse()).ToList();

        _personRepositoryMock
            .Setup(temp => temp.GetPersonsAsync())
            .ReturnsAsync(persons);


        //print person_response_list_from_add
        _testOutputHelper.WriteLine("Expected:");
        foreach (PersonResponse personResponseFromAdd in personResponseListExpected)
        {
            _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
        }

        var allPersons = await _personService.GetAllPersonsAsync();

        //Act
        var personsListFromSort =
            _personService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.Desc);

        //print persons_list_from_get
        _testOutputHelper.WriteLine("Actual:");
        foreach (var personResponseFromGet in personsListFromSort)
        {
            _testOutputHelper.WriteLine(personResponseFromGet.ToString());
        }

        //Assert
        personsListFromSort.Should().BeInDescendingOrder(temp => temp.PersonName);
    }

    #endregion


    #region UpdatePerson

    //When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
    [Fact]
    public async Task UpdatePerson_NullPerson_ToBeArgumentNullException()
    {
        //Arrange
        PersonUpdateRequest? personUpdateRequest = null;

        //Act
        var action = async () => { await _personService.UpdatePersonAsync(personUpdateRequest); };

        //Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }


    //When we supply invalid person id, it should throw ArgumentException
    [Fact]
    public async Task UpdatePerson_InvalidPersonID_ToBeArgumentException()
    {
        //Arrange
        PersonUpdateRequest? personUpdateRequest = _fixture.Build<PersonUpdateRequest>()
            .Create();

        //Act
        Func<Task> action = async () => { await _personService.UpdatePersonAsync(personUpdateRequest); };

        //Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }


    //When PersonName is null, it should throw ArgumentException
    [Fact]
    public async Task UpdatePerson_PersonNameIsNull_ToBeArgumentException()
    {
        //Arrange
        var person = _fixture.Build<Person>()
            .With(temp => temp.PersonName, null as string)
            .With(temp => temp.Email, "someone@example.com")
            .With(temp => temp.Country, null as Country)
            .With(temp => temp.Gender, "Male")
            .Create();

        var personResponseFromAdd = person.ToPersonResponse();

        var personUpdateRequest = personResponseFromAdd.ToPersonUpdateRequest();


        //Act
        var action = async () => { await _personService.UpdatePersonAsync(personUpdateRequest); };

        //Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }


    //First, add a new person and try to update the person name and email
    [Fact]
    public async Task UpdatePerson_PersonFullDetails_ToBeSuccessful()
    {
        //Arrange
        var person = _fixture.Build<Person>()
            .With(temp => temp.Email, "someone@example.com")
            .With(temp => temp.Country, null as Country)
            .With(temp => temp.Gender, "Male")
            .Create();

        var personResponseExpected = person.ToPersonResponse();

        var personUpdateRequest = personResponseExpected.ToPersonUpdateRequest();

        _personRepositoryMock
            .Setup(temp => temp.UpdatePersonAsync(It.IsAny<Person>()))
            .ReturnsAsync(person);

        _personRepositoryMock
            .Setup(temp => temp.GetPersonByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(person);

        //Act
        var personResponseFromUpdate = await _personService.UpdatePersonAsync(personUpdateRequest);

        //Assert
        personResponseFromUpdate.Should().Be(personResponseExpected);
    }

    #endregion


    #region DeletePerson

    //If you supply an valid PersonID, it should return true
    [Fact]
    public async Task DeletePerson_ValidPersonID_ToBeSuccessful()
    {
        //Arrange
        var person = _fixture.Build<Person>()
            .With(temp => temp.Email, "someone@example.com")
            .With(temp => temp.Country, null as Country)
            .With(temp => temp.Gender, "Female")
            .Create();


        _personRepositoryMock
            .Setup(temp => temp.DeletePersonByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        _personRepositoryMock
            .Setup(temp => temp.GetPersonByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(person);

        //Act
        var isDeleted = await _personService.DeletePersonAsync(person.PersonId);

        //Assert
        isDeleted.Should().BeTrue();
    }


    //If you supply an invalid PersonID, it should return false
    [Fact]
    public async Task DeletePerson_InvalidPersonID()
    {
        //Act
        bool isDeleted = await _personService.DeletePersonAsync(Guid.NewGuid());

        //Assert
        isDeleted.Should().BeFalse();
    }

    #endregion
}