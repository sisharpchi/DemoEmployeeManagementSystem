using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations;

public class SanctionTypeRepository(AppDbContext appDbContext) : IGenericRepositoryInterface<SancitonType>
{
    public async Task<GeneralRepsonse> DeleteById(int id)
    {
        var item = await GetById(id);
        if (item is null) return NotFound();
        appDbContext.SanctionTypes.Remove(item);
        await Commit();
        return Success();
    }

    public async Task<List<SancitonType>> GetAll() =>
        await appDbContext.SanctionTypes.AsNoTracking().ToListAsync();

    public async Task<SancitonType> GetById(int id) =>
        await appDbContext.SanctionTypes.FindAsync(id);

    public async Task<GeneralRepsonse> Insert(SancitonType item)
    {
        if (!await CheckName(item.Name!))
            return new GeneralRepsonse(false, "Sanction Type already exists");

        appDbContext.SanctionTypes.Add(item);
        await Commit();
        return Success();
    }

    public async Task<GeneralRepsonse> Update(SancitonType item)
    {
        var obj = await appDbContext.SanctionTypes.FindAsync(item.Id);
        if (obj is null) return NotFound();

        obj.Name = item.Name;
        await Commit();
        return Success();
    }

    private static GeneralRepsonse NotFound() =>
        new(false, "Sorry, sanction type not found");

    private static GeneralRepsonse Success() =>
        new(true, "Process completed");

    private async Task Commit() =>
        await appDbContext.SaveChangesAsync();

    private async Task<bool> CheckName(string name)
    {
        var item = await appDbContext.SanctionTypes
            .FirstOrDefaultAsync(x => x.Name!.ToLower() == name.ToLower());
        return item is null;
    }
}
