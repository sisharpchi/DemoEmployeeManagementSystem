using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations;

public class CountryRepository(AppDbContext appDbContext) : IGenericRepositoryInterface<Country>
{
    public async Task<GeneralRepsonse> DeleteById(int id)
    {
        var dep = await appDbContext.Countries.FindAsync(id);
        if (dep is null) return NotFound();
        appDbContext.Countries.Remove(dep);
        await Commit();
        return Success();
    }

    public async Task<List<Country>> GetAll() => await appDbContext.Countries.ToListAsync();

    public async Task<Country> GetById(int id) => await appDbContext.Countries.FindAsync(id);

    public async Task<GeneralRepsonse> Insert(Country item)
    {
        if (!await CheckName(item.Name!)) return new GeneralRepsonse(false, "Country already added");
        await appDbContext.Countries.AddAsync(item);
        await Commit();
        return Success();
    }

    public async Task<GeneralRepsonse> Update(Country item)
    {
        var dep = await appDbContext.Countries.FindAsync(item.Id);
        if (dep is null) return NotFound();
        dep.Name = item.Name;
        await Commit();
        return Success();
    }

    private static GeneralRepsonse NotFound() => new(false, "Sorry Country not found");
    private static GeneralRepsonse Success() => new(true, "Process completed");
    private async Task Commit() => await appDbContext.SaveChangesAsync();
    private async Task<bool> CheckName(string name)
    {
        var item = await appDbContext.Countries.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));
        return item is null;
    }
}
