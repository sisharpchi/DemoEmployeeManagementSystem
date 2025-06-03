namespace Client.ApplicationStates;

public class AllState
{
    //Scope action
    public Action? GeneralDepartmentAction { get; set; }

    //General department
    public bool ShowGeneralDepartment { get; set; }
    public void GeneralDepartmentClicked()
    {
        ResetAllDepartments();
        ShowGeneralDepartment = true;
        GeneralDepartmentAction?.Invoke();
    }

    //Department
    public bool ShowDepartment { get; set; }
    public void DepartmentClicked()
    {
        ResetAllDepartments();
        ShowDepartment = true;
        Action?.Invoke();
    }

    //Country
    public bool ShowCountry { get; set; }
    public void CountryClicked()
    {
        ResetAllDepartments();
        ShowCountry = true;
        Action?.Invoke();
    }

    //City
    public bool ShowCity { get; set; }
    public void CityClicked()
    {
        ResetAllDepartments();
        ShowCity = true;
        Action?.Invoke();
    }

    //Town
    public bool ShowTown { get; set; }
    public void TownClicked()
    {
        ResetAllDepartments();
        ShowTown = true;
        Action?.Invoke();
    }

    //User
    public bool ShowUser { get; set; }
    public void UserClicked()
    {
        ResetAllDepartments();
        ShowUser = true;
        Action?.Invoke();
    }

    //Employee
    public bool ShowEmployee { get; set; }
    public void EmployeeClicked()
    {
        ResetAllDepartments();
        ShowEmployee = true;
        Action?.Invoke();
    }

    private void ResetAllDepartments()
    {
        ShowGeneralDepartment = false;   
    }
}
