using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations;

public class DepartmentRepository(AppDbContext appDbContext) : IGenericRepositoryInterface<Department>
{
    public async Task<GeneralRepsonse> DeleteById(int id)
    {
        var dep = await appDbContext.Departments.FindAsync(id);
        if (dep is null) return NotFound();
        appDbContext.Departments.Remove(dep);
        await Commit();
        return Success();
    }

    public async Task<List<Department>> GetAll() => await appDbContext.Departments.AsNoTracking().Include(gd => gd.GeneralDepartment).ToListAsync();
    public async Task<Department> GetById(int id) => await appDbContext.Departments.FindAsync(id);

    public async Task<GeneralRepsonse> Insert(Department item)
    {
        if (!await CheckName(item.Name!)) return new GeneralRepsonse(false, "Department already added");
        appDbContext.Departments.Add(item);
        await Commit();
        return Success();
    }

    public async Task<GeneralRepsonse> Update(Department item)
    {
        var dep = await appDbContext.Departments.FindAsync(item.Id);
        if (dep is null) return NotFound();
        dep.Name = item.Name;
        dep.GeneralDepartmentId = item.GeneralDepartmentId;
        await Commit();
        return Success();
    }

    private static GeneralRepsonse NotFound() => new(false, "Sorry department not found");
    private static GeneralRepsonse Success() => new(true, "Process completed");
    private async Task Commit() => await appDbContext.SaveChangesAsync();
    private async Task<bool> CheckName(string name)
    {
        var item = await appDbContext.Departments.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));
        return item is null;
    }
}
