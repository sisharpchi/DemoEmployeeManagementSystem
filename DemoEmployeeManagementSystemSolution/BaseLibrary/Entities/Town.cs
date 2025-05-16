namespace BaseLibrary.Entities;

public class Town : BaseEntity
{
    // Many to one relationship with City
    public int CityId { get; set; }
    public City? City { get; set; }
}
