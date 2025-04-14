using AutoFixture;
using Crud.Domain.Entities;
using Crud.Core.DTOs;
using Crud.Core.ServiceContracts;
using Crud.Core.Services;
using Crud.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;

namespace Crud.Tests;

public class CountriesServiceTest
{
    private readonly ICountriesService _countriesService;
    private readonly IFixture _fixture;

    public CountriesServiceTest()
    {
        var initialCountries = new List<Country>();
        if (initialCountries == null) throw new ArgumentNullException(nameof(initialCountries));

        var dbContextMock =
            new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);

        var dbContext = dbContextMock.Object;
        dbContextMock.CreateDbSetMock(x => x.Countries, initialCountries);

        _countriesService = new CountriesService(null);
        _fixture = new Fixture();
    }

    #region AddCountry

    //When CountryAddRequest is null, it should throw ArgumentNullException
    [Fact]
    public async Task AddCountry_NullCountry()
    {
        //Arrange
        CountryAddRequest? request = null;

        //Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            //Act
            await _countriesService.AddCountryAsync(request);
        });
    }

    //When the CountryName is null, it should throw ArgumentException
    [Fact]
    public async Task AddCountry_CountryNameIsNull()
    {
        //Arrange
        var request = _fixture.Build<CountryAddRequest>().Without(x => x.CountryName).Create();

        //Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            //Act
            await _countriesService.AddCountryAsync(request);
        });
    }


    //When the CountryName is duplicate, it should throw ArgumentException
    [Fact]
    public async Task AddCountry_DuplicateCountryName()
    {
        //Arrange
        var request1 = _fixture.Build<CountryAddRequest>()
            .With(x => x.CountryName, "Test")
            .Create();
        var request2 = _fixture.Build<CountryAddRequest>()
            .With(x => x.CountryName, "Test")
            .Create();

        //Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            //Act
            await _countriesService.AddCountryAsync(request1);
            await _countriesService.AddCountryAsync(request2);
        });
    }


    //When you supply proper country name, it should insert (add) the country to the existing list of countries
    [Fact]
    public async Task AddCountry_ProperCountryDetails()
    {
        //Arrange
        var request = _fixture.Build<CountryAddRequest>().Create();

        //Act
        var response = await _countriesService.AddCountryAsync(request);
        var countriesFromGetAllCountries = await _countriesService.GetAllCountriesAsync();

        //Assert
        Assert.True(response?.CountryId != Guid.Empty);
        Assert.Contains(response, countriesFromGetAllCountries);
    }

    #endregion


    #region GetAllCountries

    [Fact]
    //The list of countries should be empty by default (before adding any countries)
    public async Task GetAllCountries_EmptyList()
    {
        //Act
        var actualCountryResponseList = await _countriesService.GetAllCountriesAsync();

        //Assert
        Assert.Empty(actualCountryResponseList);
    }

    [Fact]
    public async Task GetAllCountries_AddFewCountries()
    {
        //Arrange
        var countryRequestList = new List<CountryAddRequest>()
        {
            new() { CountryName = "USA" },
            new() { CountryName = "UK" }
        };

        //Act
        var countriesListFromAddCountry = new List<CountryResponse?>();

        foreach (var countryRequest in countryRequestList)
        {
            countriesListFromAddCountry.Add(await _countriesService.AddCountryAsync(countryRequest));
        }

        var actualCountryResponseList = await _countriesService.GetAllCountriesAsync();

        //read each element from countries_list_from_add_country
        foreach (var expectedCountry in countriesListFromAddCountry)
        {
            Assert.Contains(expectedCountry, actualCountryResponseList);
        }
    }

    #endregion


    #region GetCountryByCountryID

    [Fact]
    //If we supply null as CountryID, it should return null as CountryResponse
    public async Task GetCountryByCountryID_NullCountryID()
    {
        //Arrange
        Guid? countrId = null;

        //Act
        var countryResponseFromGetMethod = await _countriesService.GetCountryByIdAsync(countrId);


        //Assert
        Assert.Null(countryResponseFromGetMethod);
    }


    [Fact]
    //If we supply a valid country id, it should return the matching country details as CountryResponse object
    public async Task GetCountryByCountryID_ValidCountryID()
    {
        //Arrange
        var countryAddRequest = _fixture.Build<CountryAddRequest>().Create();
        var countryResponseFromAdd = await _countriesService.AddCountryAsync(countryAddRequest);

        //Act
        var countryResponseFromGet =
            await _countriesService.GetCountryByIdAsync(countryResponseFromAdd?.CountryId) ??
            throw new ArgumentNullException(
                "_countriesService.GetCountryByIdAsync(countryResponseFromAdd.CountryId)");

        //Assert
        Assert.Equal(countryResponseFromAdd, countryResponseFromGet);
    }

    #endregion
}