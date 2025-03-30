using System.ComponentModel.DataAnnotations.Schema;

namespace Crud.Core.Domain.Entities;

public class Person
{
    public Guid PersonId { get; set; }
    public string? PersonName { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public Guid? CountryId { get; set; }
    public string? Address { get; set; }
    public bool ReceiveNewsLetters { get; set; }

    [Column("TaxIdentificationNumber", TypeName = "varchar(8)")]
    public string? Tin { get; set; }

    [ForeignKey("CountryId")] public virtual Country? Country { get; set; }
}