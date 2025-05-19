using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations;

public class CityRepository(AppDbContext appDbContext) : IGenericRepositoryInterface<City>
{
    public async Task<GeneralRepsonse> DeleteById(int id)
    {
        var dep = await appDbContext.Cities.FindAsync(id);
        if (dep is null) return NotFound();
        appDbContext.Cities.Remove(dep);
        await Commit();
        return Success();
    }

    public async Task<List<City>> GetAll() => await appDbContext.Cities.ToListAsync();

    public async Task<City> GetById(int id) => await appDbContext.Cities.FindAsync(id);

    public async Task<GeneralRepsonse> Insert(City item)
    {
        if (!await CheckName(item.Name!)) return new GeneralRepsonse(false, "City already added");
        await appDbContext.Cities.AddAsync(item);
        await Commit();
        return Success();
    }

    public async Task<GeneralRepsonse> Update(City item)
    {
        var dep = await appDbContext.Cities.FindAsync(item.Id);
        if (dep is null) return NotFound();
        dep.Name = item.Name;
        await Commit();
        return Success();
    }

    private static GeneralRepsonse NotFound() => new(false, "Sorry City not found");
    private static GeneralRepsonse Success() => new(true, "Process completed");
    private async Task Commit() => await appDbContext.SaveChangesAsync();
    private async Task<bool> CheckName(string name)
    {
        var item = await appDbContext.Cities.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));
        return item is null;
    }
}
