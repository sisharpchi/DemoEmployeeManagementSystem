using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations;

public class GeneralDepartmentRepository(AppDbContext appDbContext) : IGenericRepositoryInterface<GeneralDepartment>
{
    public async Task<GeneralRepsonse> DeleteById(int id)
    {
        var dep = await appDbContext.GeneralDepartments.FindAsync(id);
        if (dep == null) return NotFound();
        appDbContext.GeneralDepartments.Remove(dep);
        await Commit();
        return Success();
    }

    public async Task<List<GeneralDepartment>> GetAll() => await appDbContext.GeneralDepartments.ToListAsync();
    public async Task<GeneralDepartment> GetById(int id) => await appDbContext.GeneralDepartments.FindAsync(id);

    public async Task<GeneralRepsonse> Insert(GeneralDepartment item)
    {
        var checkIfNull = await CheckName(item.Name);
        if (!checkIfNull) return new GeneralRepsonse(false, "General Department already added");
        await appDbContext.GeneralDepartments.AddAsync(item);
        await Commit();
        return Success();
    }

    public async Task<GeneralRepsonse> Update(GeneralDepartment item)
    {
        var dep = await appDbContext.GeneralDepartments.FindAsync(item.Id);
        if (dep == null) return NotFound();
        dep.Name = item.Name;
        await Commit();
        return Success();
    }

    private static GeneralRepsonse NotFound() => new (false, "Sorry General Department not found");
    private static GeneralRepsonse Success() => new (true, "Process completed" );
    private async Task Commit() => await appDbContext.SaveChangesAsync();
    private async Task<bool> CheckName(string name)
    {
        var item = await appDbContext.GeneralDepartments.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));
        return item is null;
    }
}
