using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations;

public class EmployeeRepository(AppDbContext appDbContext) : IGenericRepositoryInterface<Employee>
{
    public async Task<GeneralRepsonse> DeleteById(int id)
    {
        var item = await appDbContext.Employees.FindAsync(id);
        if (item is null) return NotFound();

        appDbContext.Employees.Remove(item);
        await Commit();
        return Success();
    }

    public async Task<List<Employee>> GetAll()
    {
        var employees = await appDbContext.Employees
            .AsNoTracking()
            .Include(t => t.Town)
            .ThenInclude(c => c.City)
            .ThenInclude(co => co.Country)
            .Include(b => b.Branch)
            .ThenInclude(d => d.Department)
            .ThenInclude(gd => gd.GeneralDepartment)
            .ToListAsync();

        return employees;
    }

    public async Task<Employee> GetById(int id)
    {
        var employee = await appDbContext.Employees
            .AsNoTracking()
            .Include(t => t.Town)
            .ThenInclude(c => c.City)
            .ThenInclude(co => co.Country)
            .Include(b => b.Branch)
            .ThenInclude(d => d.Department)
            .ThenInclude(gd => gd.GeneralDepartment)
            .FirstOrDefaultAsync(ei => ei.Id == id);

        return employee;
    }

    public async Task<GeneralRepsonse> Insert(Employee item)
    {
        if (!await CheckName(item.Name!)) return new GeneralRepsonse(false, "Employee already added");
        appDbContext.Employees.Add(item);
        await Commit();
        return Success();
    }

    public async Task<GeneralRepsonse> Update(Employee item)
    {
        var findUser = await appDbContext.Employees.FirstOrDefaultAsync(e => e.Id == item.Id);
        if (findUser is null) return new GeneralRepsonse(false, "Employee does not exist");

        findUser.Name = item.Name;
        findUser.Other = item.Other;
        findUser.Address = item.Address;
        findUser.TelephoneNumber = item.TelephoneNumber;
        findUser.TownId = item.TownId;
        findUser.BranchId = item.BranchId;
        findUser.CivilId = item.CivilId;
        findUser.FileNumber = item.FileNumber;
        findUser.JobName = item.JobName;
        findUser.Photo = item.Photo;

        //await appDbContext.SaveChangesAsync();
        await Commit();
        return Success();
    }

    private async Task Commit() => await appDbContext.SaveChangesAsync();
    private static GeneralRepsonse NotFound() => new(false, "Sorry employee not found");
    private static GeneralRepsonse Success() => new(true, "Process completed");
    private async Task<bool> CheckName(string name)
    {
        var item = await appDbContext.Employees.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));
        return item is null ? true : false;
    }
}
