namespace BaseLibrary.Entities;

public class VacationType : BaseEntity
{
    // Many to one relationship with Vacation
    public List<Vacation>? Vacations { get; set; }
}
