using System.Text.Json.Serialization;

namespace BaseLibrary.Entities;

public class City : BaseEntity
{
    // Many to one relationship with Country
    public int CountryId { get; set; }
    public Country? Country { get; set; }

    // One to many relationship with Town
    [JsonIgnore]
    public List<Town>? Towns { get; set; }
}
