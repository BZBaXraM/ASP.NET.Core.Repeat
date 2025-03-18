using System.Linq.Expressions;
using Entities;

namespace RepositoryContracts;

public interface IPersonsRepository
{
    Task<Person> AddPersonAsync(Person person);
    Task<Person?> GetPersonByIdAsync(Guid id);
    Task<List<Person>> GetPersonsAsync();
    Task<Person> UpdatePersonAsync(Person person);
    Task<List<Person>> GetFilteredPersonsAsync(Expression<Func<Person, bool>> filter);
    Task<bool> DeletePersonByIdAsync(Guid id);
}