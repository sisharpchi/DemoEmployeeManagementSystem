using System.Text.Json.Serialization;

namespace BaseLibrary.Entities;

public class Department : BaseEntity
{
    //Many to one relationships with General Department
    public GeneralDepartment? GeneralDepartment { get; set; } = default!;
    public int GeneralDepartmentId { get; set; }

    //One to many relationships with Branch
    [JsonIgnore]
    public List<Branch>? Branches { get; set; }
}
