using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations;

public class DoctorRepository(AppDbContext appDbContext) : IGenericRepositoryInterface<Doctor>
{
    public async Task<GeneralRepsonse> DeleteById(int id)
    {
        var item = await appDbContext.Doctors.FirstOrDefaultAsync(eid => eid.EmployeeId == id);
        if (item is null) return NotFound();
        appDbContext.Doctors.Remove(item);
        await Commit();
        return Success();
    }

    public async Task<List<Doctor>> GetAll() => await appDbContext.Doctors.AsNoTracking().ToListAsync();

    public async Task<Doctor> GetById(int id) => await appDbContext.Doctors.FirstOrDefaultAsync(eid => eid.EmployeeId == id);

    public async Task<GeneralRepsonse> Insert(Doctor item)
    {
        appDbContext.Doctors.Add(item);
        await Commit();
        return Success();
    }

    public async Task<GeneralRepsonse> Update(Doctor item)
    {
        var obj = await appDbContext.Doctors.FirstOrDefaultAsync(eid => eid.EmployeeId == item.EmployeeId);
        if (obj is null) return NotFound();
        obj.MedicalRecommendation = item.MedicalRecommendation;
        obj.MedicalDiagnose = item.MedicalDiagnose;
        obj.Date = item.Date;
        await Commit();
        return Success();
    }

    private static GeneralRepsonse NotFound() => new(false, "Sorry doctor not found");
    private static GeneralRepsonse Success() => new(true, "Process completed");
    private async Task Commit() => await appDbContext.SaveChangesAsync();
}
