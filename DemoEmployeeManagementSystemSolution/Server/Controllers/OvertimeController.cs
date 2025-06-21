using BaseLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Contracts;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OvertimeController(IGenericRepositoryInterface<Overtime> genericRepositoryInterface) : GenericController<Overtime>(genericRepositoryInterface)
{
}
