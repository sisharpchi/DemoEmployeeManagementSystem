namespace BaseLibrary.Entities;

public class Department : BaseEntity
{
    //Many to one relationships with General Department
    public int GeneralDepartmentId { get; set; }
    public GeneralDepartment? GeneralDepartment { get; set; }

    //One to many relationships with Branch
    public List<Branch>? Branches { get; set; }
}
