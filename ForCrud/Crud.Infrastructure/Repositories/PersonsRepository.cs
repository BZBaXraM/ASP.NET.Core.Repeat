using System.Linq.Expressions;
using Crud.Core.Domain.Entities;
using Crud.Core.RepositoryContracts;
using Crud.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Crud.Infrastructure.Repositories;

public class PersonsRepository : IPersonsRepository
{
    private readonly ApplicationDbContext _context;

    public PersonsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Person> AddPersonAsync(Person person)
    {
        _context.Persons.Add(person);

        await _context.SaveChangesAsync();

        return person;
    }

    public async Task<Person?> GetPersonByIdAsync(Guid id)
    {
        return await _context.Persons
            .Include("Country")
            .FirstOrDefaultAsync(p => p.PersonId == id);
    }

    public async Task<List<Person>> GetPersonsAsync()
    {
        return await _context.Persons.Include("Country").ToListAsync();
    }

    public async Task<Person> UpdatePersonAsync(Person person)
    {
        var existingPerson = await _context.Persons.FirstOrDefaultAsync(p => p.PersonId == person.PersonId);

        if (existingPerson == null)
        {
            throw new InvalidOperationException("Person not found");
        }

        existingPerson.PersonName = person.PersonName;
        existingPerson.Email = person.Email;
        existingPerson.CountryId = person.CountryId;
        existingPerson.Country = person.Country;
        existingPerson.DateOfBirth = person.DateOfBirth;
        existingPerson.Address = person.Address;
        existingPerson.Gender = person.Gender;
        existingPerson.ReceiveNewsLetters = person.ReceiveNewsLetters;
        existingPerson.Tin = person.Tin;

        await _context.SaveChangesAsync();
        return existingPerson;
    }

    public async Task<List<Person>> GetFilteredPersonsAsync(Expression<Func<Person, bool>> filter)
    {
        return await _context.Persons.Include("Country").Where(filter).ToListAsync();
    }

    public async Task<bool> DeletePersonByIdAsync(Guid id)
    {
        _context.Persons.RemoveRange(_context.Persons.Where(p => p.PersonId == id));

        var count = await _context.SaveChangesAsync();

        return count > 0;
    }
}