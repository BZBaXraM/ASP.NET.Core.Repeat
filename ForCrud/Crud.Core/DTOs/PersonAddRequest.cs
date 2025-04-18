using System.ComponentModel.DataAnnotations;
using Crud.Domain.Entities;
using Crud.Core.Enums;

namespace Crud.Core.DTOs;

public class PersonAddRequest
{
    [Required(ErrorMessage = "Person Name can't be blank")]
    public string? PersonName { get; set; }

    [Required(ErrorMessage = "Email can't be blank")]
    [EmailAddress(ErrorMessage = "Email value should be a valid email")]
    public string? Email { get; set; }

    public DateTime? DateOfBirth { get; set; }
    public GenderOptions? Gender { get; set; }
    public Guid? CountryId { get; set; }
    public string? Address { get; set; }
    public bool ReceiveNewsLetters { get; set; }

    /// <summary>
    /// Converts the current object of PersonAddRequest into a new object of Person type
    /// </summary>
    /// <returns></returns>
    public Person ToPerson()
    {
        return new Person
        {
            PersonName = PersonName, Email = Email, DateOfBirth = DateOfBirth, Gender = Gender.ToString(),
            Address = Address, CountryId = CountryId, ReceiveNewsLetters = ReceiveNewsLetters
        };
    }
}