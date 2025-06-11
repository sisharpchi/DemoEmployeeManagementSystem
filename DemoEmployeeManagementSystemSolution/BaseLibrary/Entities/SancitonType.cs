using System.Text.Json.Serialization;

namespace BaseLibrary.Entities;

public class SancitonType : BaseEntity
{
    // Many to one relationship with Vacation
    [JsonIgnore]
    public List<Sanciton>? Sancitons { get; set; }
}
