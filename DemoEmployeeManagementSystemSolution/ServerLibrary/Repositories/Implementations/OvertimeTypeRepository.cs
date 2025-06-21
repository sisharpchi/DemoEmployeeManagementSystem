using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations;

public class OvertimeTypeRepository(AppDbContext appDbContext) : IGenericRepositoryInterface<OvertimeType>
{
    public async Task<GeneralRepsonse> DeleteById(int id)
    {
        var item = await GetById(id);
        if (item is null) return NotFound();
        appDbContext.OvertimeTypes.Remove(item);
        await Commit();
        return Success();
    }

    public async Task<List<OvertimeType>> GetAll() => await appDbContext.OvertimeTypes.AsNoTracking().ToListAsync();

    public async Task<OvertimeType> GetById(int id) => await appDbContext.OvertimeTypes.FindAsync(id);

    public async Task<GeneralRepsonse> Insert(OvertimeType item)
    {
        if (!await CheckName(item.Name!)) return new GeneralRepsonse(false, "Overtime Type already added");
        appDbContext.OvertimeTypes.Add(item);
        await Commit();
        return Success();
    }

    public async Task<GeneralRepsonse> Update(OvertimeType item)
    {
        var obj = await appDbContext.Branches.FindAsync(item.Id);
        if (obj is null) return NotFound();
        obj.Name = item.Name;
        await Commit();
        return Success();
    }

    private static GeneralRepsonse NotFound() => new(false, "Sorry overtime type not found");
    private static GeneralRepsonse Success() => new(true, "Process completed");
    private async Task Commit() => await appDbContext.SaveChangesAsync();
    private async Task<bool> CheckName(string name)
    {
        var item = await appDbContext.OvertimeTypes.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));
        return item is null;
    }
}
