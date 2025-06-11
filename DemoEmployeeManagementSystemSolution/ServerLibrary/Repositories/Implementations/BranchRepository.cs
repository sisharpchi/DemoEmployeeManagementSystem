using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations;

public class BranchRepository(AppDbContext appDbContext) : IGenericRepositoryInterface<Branch>
{
    public async Task<GeneralRepsonse> DeleteById(int id)
    {
        var dep = await appDbContext.Branches.FindAsync(id);
        if (dep is null) return NotFound();
        appDbContext.Branches.Remove(dep);
        await Commit();
        return Success();
    }

    public async Task<List<Branch>> GetAll() => await appDbContext.Branches.AsNoTracking().Include(d => d.Department).ToListAsync();

    public async Task<Branch> GetById(int id) => await appDbContext.Branches.FindAsync(id);

    public async Task<GeneralRepsonse> Insert(Branch item)
    {
        if (!await CheckName(item.Name!)) return new GeneralRepsonse(false, "Branch already added");
        await appDbContext.Branches.AddAsync(item);
        await Commit();
        return Success();
    }

    public async Task<GeneralRepsonse> Update(Branch item)
    {
        var branch = await appDbContext.Branches.FindAsync(item.Id);
        if (branch is null) return NotFound();
        branch.Name = item.Name;
        branch.DepartmentId = item.DepartmentId;
        await Commit();
        return Success();
    }

    private static GeneralRepsonse NotFound() => new(false, "Sorry Branch not found");
    private static GeneralRepsonse Success() => new(true, "Process completed");
    private async Task Commit() => await appDbContext.SaveChangesAsync();
    private async Task<bool> CheckName(string name)
    {
        var item = await appDbContext.Branches.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));
        return item is null;
    }
}
