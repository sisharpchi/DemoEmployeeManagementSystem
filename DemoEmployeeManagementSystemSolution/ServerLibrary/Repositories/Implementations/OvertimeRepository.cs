using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations;

public class OvertimeRepository(AppDbContext appDbContext) : IGenericRepositoryInterface<Overtime>
{
    public async Task<GeneralRepsonse> DeleteById(int id)
    {
        var item = await appDbContext.Overtimes.FirstOrDefaultAsync(eid => eid.EmployeeId == id);
        if (item is null) return NotFound();
        appDbContext.Overtimes.Remove(item);
        await Commit();
        return Success();
    }

    public async Task<List<Overtime>> GetAll() => await appDbContext.Overtimes.AsNoTracking().Include(t => t.OvertimeType).ToListAsync();

    public async Task<Overtime> GetById(int id) => await appDbContext.Overtimes.FirstOrDefaultAsync(eid => eid.EmployeeId == id);

    public async Task<GeneralRepsonse> Insert(Overtime item)
    {
        appDbContext.Overtimes.Add(item);
        await Commit();
        return Success();
    }

    public async Task<GeneralRepsonse> Update(Overtime item)
    {
        var obj = await appDbContext.Overtimes.FirstOrDefaultAsync(eid => eid.EmployeeId == item.EmployeeId);
        if (obj is null) return NotFound();
        obj.StartDate = item.StartDate;
        obj.EndDate = item.EndDate;
        obj.OvertimeTypeId = item.OvertimeTypeId;
        await Commit();
        return Success();
    }

    private static GeneralRepsonse NotFound() => new(false, "Sorry overtime not found");
    private static GeneralRepsonse Success() => new(true, "Process completed");
    private async Task Commit() => await appDbContext.SaveChangesAsync();
}
