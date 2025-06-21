using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations;

public class VacationTypeRepository(AppDbContext appDbContext) : IGenericRepositoryInterface<VacationType>
{
    public async Task<GeneralRepsonse> DeleteById(int id)
    {
        var item = await GetById(id);
        if (item is null) return NotFound();
        appDbContext.VacationTypes.Remove(item);
        await Commit();
        return Success();
    }

    public async Task<List<VacationType>> GetAll() =>
        await appDbContext.VacationTypes.AsNoTracking().ToListAsync();

    public async Task<VacationType> GetById(int id) =>
        await appDbContext.VacationTypes.FindAsync(id);

    public async Task<GeneralRepsonse> Insert(VacationType item)
    {
        if (!await CheckName(item.Name!))
            return new GeneralRepsonse(false, "Vacation Type already exists");

        appDbContext.VacationTypes.Add(item);
        await Commit();
        return Success();
    }

    public async Task<GeneralRepsonse> Update(VacationType item)
    {
        var existing = await appDbContext.VacationTypes.FindAsync(item.Id);
        if (existing is null) return NotFound();

        existing.Name = item.Name;
        await Commit();
        return Success();
    }

    private static GeneralRepsonse NotFound() =>
        new(false, "Vacation Type not found");

    private static GeneralRepsonse Success() =>
        new(true, "Process completed");

    private async Task Commit() =>
        await appDbContext.SaveChangesAsync();

    private async Task<bool> CheckName(string name)
    {
        var existing = await appDbContext.VacationTypes
            .FirstOrDefaultAsync(x => x.Name!.ToLower() == name.ToLower());
        return existing is null;
    }
}
