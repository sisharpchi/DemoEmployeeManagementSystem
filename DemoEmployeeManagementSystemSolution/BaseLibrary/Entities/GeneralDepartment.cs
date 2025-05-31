namespace BaseLibrary.Entities;

public class GeneralDepartment : BaseEntity
{
    //One to many relationships with Department
    public List<Department>? Departments { get; set; }
}
  