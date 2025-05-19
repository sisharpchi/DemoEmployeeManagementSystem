namespace BaseLibrary.Entities;

public class SancitonType : BaseEntity
{
    // Many to one relationship with Vacation
    public List<Sanciton>? Sancitons { get; set; }
}
