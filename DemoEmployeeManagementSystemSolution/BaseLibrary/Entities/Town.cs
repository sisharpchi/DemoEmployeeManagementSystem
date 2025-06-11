using System.Text.Json.Serialization;

namespace BaseLibrary.Entities;

public class Town : BaseEntity
{
    //Relationships : one to many with Employee
    [JsonIgnore]
    public List<Employee>? Employees { get; set; }

    // Many to one relationship with City
    public int CityId { get; set; }
    public City? City { get; set; }
}
