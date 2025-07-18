﻿using System.Text.Json.Serialization;

namespace BaseLibrary.Entities;

public class Branch : BaseEntity
{
    // Many to one relationships with Department
    public int DepartmentId { get; set; }
    public Department? Department { get; set; }

    // One to many relationships with Employee
    [JsonIgnore]
    public List<Employee>? Employees { get; set; }
}
