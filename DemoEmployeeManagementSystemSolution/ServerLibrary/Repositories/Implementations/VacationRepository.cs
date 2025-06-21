using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations;

public class VacationRepository(AppDbContext appDbContext) : IGenericRepositoryInterface<Vacation>
{
    public async Task<GeneralRepsonse> DeleteById(int id)
    {
        var item = await appDbContext.Vacations.FirstOrDefaultAsync(eid => eid.EmployeeId == id);
        if (item is null) return NotFound();
        appDbContext.Vacations.Remove(item);
        await Commit();
        return Success();
    }

    public async Task<List<Vacation>> GetAll() => await appDbContext.Vacations.AsNoTracking().Include(t => t.VacationType).ToListAsync();

    public async Task<Vacation> GetById(int id) => await appDbContext.Vacations.FirstOrDefaultAsync(eid => eid.EmployeeId == id);

    public async Task<GeneralRepsonse> Insert(Vacation item)
    {
        appDbContext.Vacations.Add(item);
        await Commit();
        return Success();
    }

    public async Task<GeneralRepsonse> Update(Vacation item)
    {
        var obj = await appDbContext.Vacations.FirstOrDefaultAsync(eid => eid.EmployeeId == item.EmployeeId);
        if (obj is null) return NotFound();
        obj.StartDate = item.StartDate;
        obj.NumberOfDays = item.NumberOfDays;
        obj.VacationTypeId = item.VacationTypeId;
        await Commit();
        return Success();
    }

    private static GeneralRepsonse NotFound() => new(false, "Sorry vacation not found");
    private static GeneralRepsonse Success() => new(true, "Process completed");
    private async Task Commit() => await appDbContext.SaveChangesAsync();
}
