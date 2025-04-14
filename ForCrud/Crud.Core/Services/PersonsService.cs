using Crud.Domain.Entities;
using Crud.Core.DTOs;
using Crud.Core.Enums;
using Crud.Core.Helpers;
using Crud.Core.RepositoryContracts;
using Crud.Core.ServiceContracts;

namespace Crud.Core.Services;

public class PersonsService : IPersonsService
{
    private readonly IPersonsRepository _personsRepository;

    public PersonsService(IPersonsRepository personsRepository)
    {
        _personsRepository = personsRepository;
    }

    public async Task<PersonResponse> AddPersonAsync(PersonAddRequest? personAddRequest)
    {
        ArgumentNullException.ThrowIfNull(personAddRequest);


        if (string.IsNullOrWhiteSpace(personAddRequest.PersonName))
        {
            throw new ArgumentException("PersonName cannot be null or empty", nameof(personAddRequest.PersonName));
        }

        var person = personAddRequest.ToPerson();
        person.PersonId = Guid.NewGuid(); // Ensure a new PersonId is generated

        await _personsRepository.AddPersonAsync(person);

        return person.ToPersonResponse();
    }

    public async Task<List<PersonResponse>> GetAllPersonsAsync()
    {
        var persons = await _personsRepository.GetPersonsAsync();

        return persons.Select(temp => temp.ToPersonResponse()).ToList();
    }

    public async Task<PersonResponse?> GetPersonByPersonIdAsync(Guid? personId)
    {
        if (personId == null)
        {
            return null;
        }

        var person = await _personsRepository.GetPersonByIdAsync(personId.Value);

        return person?.ToPersonResponse();
    }

    public async Task<List<PersonResponse>> GetFilteredPersonsAsync(string searchBy, string? searchString)
    {
        List<Person> persons = searchBy switch
        {
            nameof(PersonResponse.PersonName) =>
                await _personsRepository.GetFilteredPersonsAsync(temp =>
                    temp.PersonName.Contains(searchString)),

            nameof(PersonResponse.Email) =>
                await _personsRepository.GetFilteredPersonsAsync(temp =>
                    temp.Email.Contains(searchString)),

            nameof(PersonResponse.DateOfBirth) =>
                await _personsRepository.GetFilteredPersonsAsync(temp =>
                    temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString)),


            nameof(PersonResponse.Gender) =>
                await _personsRepository.GetFilteredPersonsAsync(temp =>
                    temp.Gender.Contains(searchString)),

            nameof(PersonResponse.CountryId) =>
                await _personsRepository.GetFilteredPersonsAsync(temp =>
                    temp.Country.CountryName.Contains(searchString)),

            nameof(PersonResponse.Address) =>
                await _personsRepository.GetFilteredPersonsAsync(temp =>
                    temp.Address.Contains(searchString)),

            _ => await _personsRepository.GetPersonsAsync()
        };

        return persons.Select(temp => temp.ToPersonResponse()).ToList();
    }

    public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy,
        SortOrderOptions sortOrder)
    {
        return sortOrder switch
        {
            SortOrderOptions.Asc => sortBy switch
            {
                "PersonName" => allPersons.OrderBy(person => person.PersonName).ToList(),
                "Email" => allPersons.OrderBy(person => person.Email).ToList(),
                _ => allPersons
            },
            SortOrderOptions.Desc => sortBy switch
            {
                "PersonName" => allPersons.OrderByDescending(person => person.PersonName).ToList(),
                "Email" => allPersons.OrderByDescending(person => person.Email).ToList(),
                _ => allPersons
            },
            _ => allPersons
        };
    }

    public async Task<PersonResponse> UpdatePersonAsync(PersonUpdateRequest? personUpdateRequest)
    {
        ArgumentNullException.ThrowIfNull(personUpdateRequest);

        ValidationHelper.ModelValidation(personUpdateRequest);

        var matchingPerson = await _personsRepository.GetPersonByIdAsync(personUpdateRequest.PersonId);

        if (matchingPerson == null)
        {
            throw new ArgumentException("Given person id doesn't exist");
        }

        matchingPerson.PersonName = personUpdateRequest.PersonName;
        matchingPerson.Email = personUpdateRequest.Email;
        matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
        matchingPerson.Gender = personUpdateRequest.Gender.ToString();
        matchingPerson.CountryId = personUpdateRequest.CountryId;
        matchingPerson.Address = personUpdateRequest.Address;
        matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

        await _personsRepository.UpdatePersonAsync(matchingPerson);

        return matchingPerson.ToPersonResponse();
    }

    public async Task<bool> DeletePersonAsync(Guid? personId)
    {
        if (personId == null)
        {
            throw new ArgumentNullException(nameof(personId));
        }

        var person = await _personsRepository.GetPersonByIdAsync(personId.Value);

        if (person == null)
            return false;

        await _personsRepository.DeletePersonByIdAsync(personId.Value);

        return true;
    }
}