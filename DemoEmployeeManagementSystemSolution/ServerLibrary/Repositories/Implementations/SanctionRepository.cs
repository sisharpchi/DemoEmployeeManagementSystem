using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations;

public class SanctionRepository(AppDbContext appDbContext) : IGenericRepositoryInterface<Sanciton>
{
    public async Task<GeneralRepsonse> DeleteById(int id)
    {
        var item = await appDbContext.Sanctions.FirstOrDefaultAsync(eid => eid.EmployeeId == id);
        if (item is null) return NotFound();
        appDbContext.Sanctions.Remove(item);
        await Commit();
        return Success();
    }

    public async Task<List<Sanciton>> GetAll() => await appDbContext.Sanctions.AsNoTracking().Include(t => t.SancitonType).ToListAsync();

    public async Task<Sanciton> GetById(int id) => await appDbContext.Sanctions.FirstOrDefaultAsync(eid => eid.EmployeeId == id);

    public async Task<GeneralRepsonse> Insert(Sanciton item)
    {
        appDbContext.Sanctions.Add(item);
        await Commit();
        return Success();
    }

    public async Task<GeneralRepsonse> Update(Sanciton item)
    {
        var obj = await appDbContext.Sanctions.FirstOrDefaultAsync(eid => eid.EmployeeId == item.EmployeeId);
        if (obj is null) return NotFound();
        obj.PunishmentDate = item.PunishmentDate;
        obj.Punishment = item.Punishment;
        obj.Date = item.Date;
        obj.SancitonType = item.SancitonType;
        await Commit();
        return Success();
    }

    private static GeneralRepsonse NotFound() => new(false, "Sorry sanction not found");
    private static GeneralRepsonse Success() => new(true, "Process completed");
    private async Task Commit() => await appDbContext.SaveChangesAsync();
}
