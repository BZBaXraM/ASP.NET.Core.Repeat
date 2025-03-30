using AutoFixture;
using Crud.Api.Controllers;
using Crud.Core.DTOs;
using Crud.Core.ServiceContracts;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Crud.Tests.Controllers;

[TestSubject(typeof(CountriesController))]
public class CountriesControllerTest
{
    private readonly CountriesController _countriesController;
    private readonly Mock<ICountriesService> _countriesServiceMock;
    private readonly IFixture _fixture;

    public CountriesControllerTest()
    {
        _countriesServiceMock = new Mock<ICountriesService>();
        _countriesController = new CountriesController(_countriesServiceMock.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task GetAllCountries_ReturnsOkObjectResult()
    {
        // Arrange
        var countries = _fixture.CreateMany<CountryResponse>().ToList();
        _countriesServiceMock.Setup(x => x.GetAllCountriesAsync()).ReturnsAsync(countries);

        // Act
        var result = await _countriesController.GetAllCountries();

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<CountryResponse>>(okObjectResult.Value);
        Assert.Equal(countries, model);
    }

    [Fact]
    public async Task AddCountry_ReturnsOkObjectResult()
    {
        // Arrange
        var country = _fixture.Create<CountryAddRequest>();
        _countriesServiceMock.Setup(x => x.AddCountryAsync(It.IsAny<CountryAddRequest>()))
            .ReturnsAsync(new CountryResponse());

        // Act
        var result = await _countriesController.AddCountry(country);

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsType<CountryResponse>(okObjectResult.Value);
        Assert.NotNull(model);
    }

    [Fact]
    public async Task GetCountryById_ReturnsOkObjectResult()
    {
        // Arrange
        var country = _fixture.Create<CountryResponse>();
        _countriesServiceMock.Setup(x => x.GetCountryByIdAsync(It.IsAny<Guid?>()))
            .ReturnsAsync(country);

        // Act
        var result = await _countriesController.GetCountryById(Guid.NewGuid());

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsType<CountryResponse>(okObjectResult.Value);
        Assert.NotNull(model);
    }

    [Fact]
    public async Task GetCountryByName_ReturnsOkObjectResult()
    {
        // Arrange
        var country = _fixture.Create<CountryResponse>();
        _countriesServiceMock.Setup(x => x.GetCountryByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(country);

        // Act
        var result = await _countriesController.GetCountryByName("Country");

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsType<CountryResponse>(okObjectResult.Value);
        Assert.NotNull(model);
    }
}