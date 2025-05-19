using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations;

public class TownRepository(AppDbContext appDbContext) : IGenericRepositoryInterface<Town>
{
    public async Task<GeneralRepsonse> DeleteById(int id)
    {
        var dep = await appDbContext.Towns.FindAsync(id);
        if (dep is null) return NotFound();
        appDbContext.Towns.Remove(dep);
        await Commit();
        return Success();
    }

    public async Task<List<Town>> GetAll() => await appDbContext.Towns.ToListAsync();

    public async Task<Town> GetById(int id) => await appDbContext.Towns.FindAsync(id);

    public async Task<GeneralRepsonse> Insert(Town item)
    {
        if (!await CheckName(item.Name!)) return new GeneralRepsonse(false, "Town already added");
        await appDbContext.Towns.AddAsync(item);
        await Commit();
        return Success();
    }

    public async Task<GeneralRepsonse> Update(Town item)
    {
        var dep = await appDbContext.Cities.FindAsync(item.Id);
        if (dep is null) return NotFound();
        dep.Name = item.Name;
        await Commit();
        return Success();
    }

    private static GeneralRepsonse NotFound() => new(false, "Sorry Town not found");
    private static GeneralRepsonse Success() => new(true, "Process completed");
    private async Task Commit() => await appDbContext.SaveChangesAsync();
    private async Task<bool> CheckName(string name)
    {
        var item = await appDbContext.Towns.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));
        return item is null;
    }
}
