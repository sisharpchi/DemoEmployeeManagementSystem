﻿using BaseLibrary.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServerLibrary.Data;
using ServerLibrary.Helpers;
using ServerLibrary.Repositories.Contracts;
using ServerLibrary.Repositories.Implementations;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtSection>(builder.Configuration.GetSection("JwtSection"));
var jwtSection = builder.Configuration.GetSection(nameof(JwtSection)).Get<JwtSection>();
// starting
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Sorry, your connection is not found"));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorWasm",
        builder => builder
            .WithOrigins("https://localhost:7134", "https://localhost:7285")
            .AllowAnyMethod()
            .AllowAnyHeader());

});

var app = builder.Build();


//app.MapScalarApiReference();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazorWasm");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

//2
//using BaseLibrary.Entities;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using ServerLibrary.Data;
//using ServerLibrary.Helpers;
//using ServerLibrary.Repositories.Contracts;
//using ServerLibrary.Repositories.Implementations;
//using System.Text;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//// JWT konfiguratsiya
//builder.Services.Configure<JwtSection>(builder.Configuration.GetSection("JwtSection"));
//var jwtSection = builder.Configuration.GetSection(nameof(JwtSection)).Get<JwtSection>();

//// DB context
//builder.Services.AddDbContext<AppDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
//        ?? throw new InvalidOperationException("Sorry, your connection is not found"));
//});

//// JWT autentifikatsiya
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters()
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateIssuerSigningKey = true,
//        ValidateLifetime = true,
//        ValidIssuer = jwtSection!.Issuer,
//        ValidAudience = jwtSection.Audience,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection.Key!))
//    };
//});

//// Repositoriyalar
//builder.Services.AddScoped<IUserAccount, UserAccountRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<GeneralDepartment>, GeneralDepartmentRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<Department>, DepartmentRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<Branch>, BranchRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<Country>, CountryRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<City>, CityRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<Town>, TownRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<Overtime>, OvertimeRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<OvertimeType>, OvertimeTypeRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<Sanciton>, SanctionRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<SancitonType>, SanctionTypeRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<Vacation>, VacationRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<VacationType>, VacationTypeRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<Doctor>, DoctorRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<Employee>, EmployeeRepository>();

//// CORS
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowBlazorWasm",
//        builder => builder
//            .WithOrigins("https://localhost:7134", "https://localhost:7285")
//            .AllowAnyMethod()
//            .AllowAnyHeader());
//});

//// ⚠️ Blazor static fayllar uchun
//builder.Services.AddSpaStaticFiles(configuration =>
//{
//    configuration.RootPath = "wwwroot"; // publishda WASM fayllari shu yerga tushadi
//});

//var app = builder.Build();

//// Swagger
//if (app.Environment.IsDevelopment() || true)
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles(); // static fayllarni berish uchun
//app.UseSpaStaticFiles(); // Blazor uchun
//app.UseCors("AllowBlazorWasm");

//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();

//// ✅ SPA fallback routing: frontend URL larini ishlashi uchun
//app.MapFallbackToFile("index.html");

//app.Run();


//3
//using BaseLibrary.Entities;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using ServerLibrary.Data;
//using ServerLibrary.Helpers;
//using ServerLibrary.Repositories.Contracts;
//using ServerLibrary.Repositories.Implementations;
//using System.Text;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//// JWT konfiguratsiya
//builder.Services.Configure<JwtSection>(builder.Configuration.GetSection("JwtSection"));
//var jwtSection = builder.Configuration.GetSection(nameof(JwtSection)).Get<JwtSection>();

//// DB context
//builder.Services.AddDbContext<AppDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
//        ?? throw new InvalidOperationException("Sorry, your connection is not found"));
//});

//// JWT autentifikatsiya
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters()
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateIssuerSigningKey = true,
//        ValidateLifetime = true,
//        ValidIssuer = jwtSection!.Issuer,
//        ValidAudience = jwtSection.Audience,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection.Key!))
//    };
//});

//// Repositoriyalar
//builder.Services.AddScoped<IUserAccount, UserAccountRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<GeneralDepartment>, GeneralDepartmentRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<Department>, DepartmentRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<Branch>, BranchRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<Country>, CountryRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<City>, CityRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<Town>, TownRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<Overtime>, OvertimeRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<OvertimeType>, OvertimeTypeRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<Sanciton>, SanctionRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<SancitonType>, SanctionTypeRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<Vacation>, VacationRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<VacationType>, VacationTypeRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<Doctor>, DoctorRepository>();
//builder.Services.AddScoped<IGenericRepositoryInterface<Employee>, EmployeeRepository>();

//// CORS
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowBlazorWasm",
//        builder => builder
//            .WithOrigins("https://localhost:7134", "https://localhost:7285")
//            .AllowAnyMethod()
//            .AllowAnyHeader());
//});

//// Blazor static fayllar uchun
//builder.Services.AddSpaStaticFiles(configuration =>
//{
//    configuration.RootPath = "wwwroot"; // <-- aynan Server loyihasidagi wwwroot
//});

//var app = builder.Build();

//// Swagger
//if (app.Environment.IsDevelopment() || true)
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseSpaStaticFiles();

//app.UseRouting(); // 🔥 MUHIM QO‘SHILISHI KERAK

//app.UseCors("AllowBlazorWasm");
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();

//// ✅ Fallback routing for Blazor
//app.MapFallbackToFile("index.html");

//app.Run();
