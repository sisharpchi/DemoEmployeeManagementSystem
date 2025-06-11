using System.Text.Json.Serialization;

namespace BaseLibrary.Entities;

public class OvertimeType : BaseEntity
{
    // Many to one relationship with Overtime
    [JsonIgnore]
    public List<Overtime>? Overtimes { get; set; }
}
