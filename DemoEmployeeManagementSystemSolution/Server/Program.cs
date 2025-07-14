using BaseLibrary.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServerLibrary.Data;
using ServerLibrary.Helpers;
using ServerLibrary.Repositories.Contracts;
using ServerLibrary.Repositories.Implementations;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- Services ---

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtSection>(builder.Configuration.GetSection("JwtSection"));
var jwtSection = builder.Configuration.GetSection(nameof(JwtSection)).Get<JwtSection>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = jwtSection!.Issuer,
        ValidAudience = jwtSection.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection.Key!))
    };
});

// --- Repositories ---

builder.Services.AddScoped<IUserAccount, UserAccountRepository>();

builder.Services.AddScoped<IGenericRepositoryInterface<GeneralDepartment>, GeneralDepartmentRepository>();
builder.Services.AddScoped<IGenericRepositoryInterface<Department>, DepartmentRepository>();
builder.Services.AddScoped<IGenericRepositoryInterface<Branch>, BranchRepository>();
builder.Services.AddScoped<IGenericRepositoryInterface<Country>, CountryRepository>();
builder.Services.AddScoped<IGenericRepositoryInterface<City>, CityRepository>();
builder.Services.AddScoped<IGenericRepositoryInterface<Town>, TownRepository>();
builder.Services.AddScoped<IGenericRepositoryInterface<Overtime>, OvertimeRepository>();
builder.Services.AddScoped<IGenericRepositoryInterface<OvertimeType>, OvertimeTypeRepository>();
builder.Services.AddScoped<IGenericRepositoryInterface<Sanciton>, SanctionRepository>();
builder.Services.AddScoped<IGenericRepositoryInterface<SancitonType>, SanctionTypeRepository>();
builder.Services.AddScoped<IGenericRepositoryInterface<Vacation>, VacationRepository>();
builder.Services.AddScoped<IGenericRepositoryInterface<VacationType>, VacationTypeRepository>();
builder.Services.AddScoped<IGenericRepositoryInterface<Doctor>, DoctorRepository>();
builder.Services.AddScoped<IGenericRepositoryInterface<Employee>, EmployeeRepository>();

// --- CORS ---

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorWasm", policy =>
    {
        policy.WithOrigins("https://localhost:7134", "https://localhost:7285")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// --- Blazor WASM static files support ---
builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "Client/wwwroot"; // <-- Blazor WebAssembly build natijasi shu yerda bo'lishi kerak
});

var app = builder.Build();

// --- Middleware Pipeline ---

if (app.Environment.IsDevelopment() || true) // || true for Azure prod preview
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowBlazorWasm");

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles(); // <-- umumiy wwwroot uchun
app.UseSpaStaticFiles(); // <-- faqat Client/wwwroot

app.UseRouting();

app.MapControllers(); // <-- API'lar uchun

// --- Blazor fallback (SPA routing) ---
app.MapFallbackToFile("index.html"); // <-- Blazor frontni chaqirish uchun

app.Run();
