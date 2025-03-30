using AutoFixture;
using Crud.Api.Controllers;
using Crud.Core.DTOs;
using Crud.Core.Enums;
using Crud.Core.ServiceContracts;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Crud.Tests.Controllers;

[TestSubject(typeof(PersonsController))]
public class PersonsControllerTest
{
    private readonly IPersonsService _personsService;
    private readonly ICountriesService _countriesService;
    private readonly Mock<IPersonsService> _personsServiceMock;
    private readonly Mock<ICountriesService> _countriesServiceMock;
    private readonly IFixture _fixture;

    public PersonsControllerTest()
    {
        _personsServiceMock = new Mock<IPersonsService>();
        _countriesServiceMock = new Mock<ICountriesService>();
        _personsService = _personsServiceMock.Object;
        _countriesService = _countriesServiceMock.Object;

        _fixture = new Fixture();
    }

    [Fact]
    public async Task GetAllPersons_ReturnsOkObjectResult()
    {
        // Arrange
        var persons = _fixture.CreateMany<PersonResponse>().ToList();
        _personsServiceMock.Setup(x => x.GetAllPersonsAsync()).ReturnsAsync(persons);

        var controller = new PersonsController(_personsService, _countriesService);

        // Act
        var result = await controller.GetAllPersons();

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<PersonResponse>>(okObjectResult.Value);
        Assert.Equal(persons, model);
    }

    [Fact]
    public async Task AddPerson_ReturnsOkObjectResult()
    {
        // Arrange
        var person = _fixture.Create<PersonAddRequest>();
        _personsServiceMock.Setup(x => x.AddPersonAsync(It.IsAny<PersonAddRequest>()))
            .ReturnsAsync(new PersonResponse());
        var controller = new PersonsController(_personsService, _countriesService);

        // Act
        var result = await controller.AddPerson(person);

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsType<PersonResponse>(okObjectResult.Value);
        Assert.NotNull(model);
    }

    [Fact]
    public async Task GetPersonById_ReturnsOkObjectResult()
    {
        // Arrange
        var person = _fixture.Create<PersonResponse>();
        _personsServiceMock.Setup(x => x.GetPersonByPersonIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(person);
        var controller = new PersonsController(_personsService, _countriesService);

        // Act
        var result = await controller.GetPersonById(person.PersonId);

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsType<PersonResponse>(okObjectResult.Value);
        Assert.NotNull(model);
    }

    [Fact]
    public async Task GetPersonById_ReturnsNotFound()
    {
        // Arrange
        _personsServiceMock.Setup(x => x.GetPersonByPersonIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((PersonResponse)null);
        var controller = new PersonsController(_personsService, _countriesService);

        // Act
        var result = await controller.GetPersonById(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetFilteredPersons_ReturnsOkObjectResult()
    {
        // Arrange
        var persons = _fixture.CreateMany<PersonResponse>().ToList();
        _personsServiceMock.Setup(x => x.GetFilteredPersonsAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(persons);
        var controller = new PersonsController(_personsService, _countriesService);

        // Act
        var result = await controller.GetFilteredPersons("searchBy", "searchString");

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<PersonResponse>>(okObjectResult.Value);
        Assert.Equal(persons, model);
    }

    [Fact]
    public async Task GetSortedPersons_ReturnsOkObjectResult()
    {
        // Arrange
        var persons = _fixture.CreateMany<PersonResponse>().ToList();
        _personsServiceMock
            .Setup(x => x.GetSortedPersons(It.IsAny<List<PersonResponse>>(), "sortBy", SortOrderOptions.Asc))
            .Returns(persons);
        var controller = new PersonsController(_personsService, _countriesService);

        // Act
        var result = await controller.GetSortedPersons("sortBy", SortOrderOptions.Asc);

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<PersonResponse>>(okObjectResult.Value);
        Assert.Equal(persons, model);
    }

    [Fact]
    public async Task UpdatePerson_ReturnsOkObjectResult()
    {
        // Arrange
        var person = _fixture.Create<PersonUpdateRequest>();
        _personsServiceMock.Setup(x => x.UpdatePersonAsync(It.IsAny<PersonUpdateRequest>()))
            .ReturnsAsync(new PersonResponse());
        var controller = new PersonsController(_personsService, _countriesService);

        // Act
        var result = await controller.UpdatePerson(person);

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsType<PersonResponse>(okObjectResult.Value);
        Assert.NotNull(model);
    }

    [Fact]
    public async Task DeletePerson_ReturnsOkObjectResult()
    {
        // Arrange
        var personId = Guid.NewGuid();
        _personsServiceMock.Setup(x => x.DeletePersonAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);
        var controller = new PersonsController(_personsService, _countriesService);

        // Act
        var result = await controller.DeletePerson(personId);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
        Assert.NotNull(okResult);
    }
}