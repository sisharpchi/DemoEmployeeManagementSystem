namespace BaseLibrary.Entities;

public class Country : BaseEntity
{
    // One to many relationship with City
    public List<City>? Cities { get; set; }
}
