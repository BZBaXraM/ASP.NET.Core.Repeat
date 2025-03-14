using Entities;
using ServiceContracts;
using ServiceContracts.DTOs;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services;

public class PersonsService : IPersonsService
{
    private readonly List<Person> _persons;
    private readonly ICountriesService _countriesService;

    public PersonsService()
    {
        _persons = [];
        _countriesService = new CountriesService();
    }

    public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
    {
        ArgumentNullException.ThrowIfNull(personAddRequest);

        ValidationHelper.ModelValidation(personAddRequest);

        var person = personAddRequest.ToPerson();

        person.PersonId = Guid.NewGuid();

        _persons.Add(person);

        return ConvertPersonToPersonResponse(person);
    }

    public List<PersonResponse> GetAllPersons()
    {
        return _persons.Select(ConvertPersonToPersonResponse).ToList();
    }

    public PersonResponse? GetPersonByPersonId(Guid? personId)
    {
        return _persons.FirstOrDefault(temp => temp.PersonId == personId)?.ToPersonResponse();
    }

    public IReadOnlyList<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
    {
        var allPersons = GetAllPersons();

        if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            return allPersons;

        Func<PersonResponse, bool> predicate = searchBy switch
        {
            "PersonName" => temp =>
                !string.IsNullOrEmpty(temp.PersonName) &&
                temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase),
            "Email" => temp =>
                !string.IsNullOrEmpty(temp.Email) &&
                temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase),
            "DateOfBirth" => temp => temp.DateOfBirth != null && temp.DateOfBirth.Value.ToString("dd MMMM yyyy")
                .Contains(searchString, StringComparison.OrdinalIgnoreCase),
            "Gender" => temp =>
                !string.IsNullOrEmpty(temp.Gender) &&
                temp.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase),
            "CountryId" => temp =>
                !string.IsNullOrEmpty(temp.Country) &&
                temp.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase),
            "Address" => temp =>
                !string.IsNullOrEmpty(temp.Address) &&
                temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase),
            _ => temp => true
        };

        return allPersons.Where(predicate).ToList();
    }

    public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy,
        SortOrderOptions sortOrder)
    {
        if (string.IsNullOrEmpty(sortBy))
            return allPersons;

        var sortedPersons = sortBy switch
        {
            nameof(PersonResponse.PersonName) => sortOrder == SortOrderOptions.Asc
                ? allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList()
                : allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

            nameof(PersonResponse.Email) => sortOrder == SortOrderOptions.Asc
                ? allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList()
                : allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

            nameof(PersonResponse.DateOfBirth) => sortOrder == SortOrderOptions.Asc
                ? allPersons.OrderBy(temp => temp.DateOfBirth).ToList()
                : allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

            nameof(PersonResponse.Age) => sortOrder == SortOrderOptions.Asc
                ? allPersons.OrderBy(temp => temp.Age).ToList()
                : allPersons.OrderByDescending(temp => temp.Age).ToList(),

            nameof(PersonResponse.Gender) => sortOrder == SortOrderOptions.Asc
                ? allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList()
                : allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

            nameof(PersonResponse.Country) => sortOrder == SortOrderOptions.Asc
                ? allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList()
                : allPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

            nameof(PersonResponse.Address) => sortOrder == SortOrderOptions.Asc
                ? allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList()
                : allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

            nameof(PersonResponse.ReceiveNewsLetters) => sortOrder == SortOrderOptions.Asc
                ? allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList()
                : allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

            _ => allPersons
        };

        return sortedPersons;
    }

    public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
    {
        ArgumentNullException.ThrowIfNull(personUpdateRequest);

        ValidationHelper.ModelValidation(personUpdateRequest);

        var matchingPerson = _persons.FirstOrDefault(temp => temp.PersonId == personUpdateRequest.PersonId);
        if (matchingPerson == null)
        {
            throw new ArgumentException("Given person id doesn't exist");
        }

        matchingPerson.PersonName = personUpdateRequest.PersonName;
        matchingPerson.Email = personUpdateRequest.Email;
        matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
        matchingPerson.Gender = personUpdateRequest.Gender.ToString();
        matchingPerson.CountryId = personUpdateRequest.CountryID;
        matchingPerson.Address = personUpdateRequest.Address;
        matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

        return matchingPerson.ToPersonResponse();
    }

    public bool DeletePerson(Guid? personId)
    {
        var person = _persons.FirstOrDefault(temp => temp.PersonId == personId);

        if (person == null)
        {
            throw new ArgumentException("Person not found");
        }

        return _persons.Remove(person);
    }

    private PersonResponse ConvertPersonToPersonResponse(Person person)
    {
        var personResponse = person.ToPersonResponse();
        personResponse.Country = _countriesService.GetCountryById(person.CountryId)?.CountryName;
        return personResponse;
    }
}