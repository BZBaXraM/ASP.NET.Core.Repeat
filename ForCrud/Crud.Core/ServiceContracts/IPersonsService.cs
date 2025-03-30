using Crud.Core.DTOs;
using Crud.Core.Enums;

namespace Crud.Core.ServiceContracts;

public interface IPersonsService
{
    Task<PersonResponse> AddPersonAsync(PersonAddRequest? personAddRequest);
    Task<List<PersonResponse>> GetAllPersonsAsync();
    Task<PersonResponse?> GetPersonByPersonIdAsync(Guid? personId);
    Task<List<PersonResponse>> GetFilteredPersonsAsync(string searchBy, string? searchString);

    List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy,
        SortOrderOptions sortOrder);

    Task<PersonResponse> UpdatePersonAsync(PersonUpdateRequest? personUpdateRequest);
    Task<bool> DeletePersonAsync(Guid? personId);
}