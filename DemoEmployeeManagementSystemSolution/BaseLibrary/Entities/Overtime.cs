using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities;

public class Overtime : OtherBaseEntity
{
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    public int NumberOfDays => (EndDate - StartDate).Days;

    // Many to one relationship with Vaction Type
    public OvertimeType? OvertimeType { get; set; }
    [Required]
    public int OvertimeTypeId { get; set; }
}
