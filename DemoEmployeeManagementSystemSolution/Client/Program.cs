using BaseLibrary.Entities;
using Blazored.LocalStorage;
using Client;
using Client.ApplicationStates;
using ClientLibrary.Helpers;
using ClientLibrary.Services.Contracts;
using ClientLibrary.Services.Implementations;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Popups;
using Syncfusion.Licensing;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

SyncfusionLicenseProvider.RegisterLicense("MzkxNDAwNEAzMjMzMmUzMDJlMzBUaTlUTldYT045TThHbmNkbUxtMUdnR1VxYnkxWXBMNHpxaVdqRHVXQnQ0PQ==");


builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<CustomHttpHandler>();
builder.Services.AddHttpClient("SystemApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7134/");
}).AddHttpMessageHandler<CustomHttpHandler>();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7134/") });

builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<GetHttpClient>();
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();

// GeneralDepartment, Department, Branch
builder.Services.AddScoped<IGenericServiceInterface<GeneralDepartment>, GenericSeriveImplemantation<GeneralDepartment>>();
builder.Services.AddScoped<IGenericServiceInterface<Department>, GenericSeriveImplemantation<Department>>();
builder.Services.AddScoped<IGenericServiceInterface<Branch>, GenericSeriveImplemantation<Branch>>();

// Country, City, Town
builder.Services.AddScoped<IGenericServiceInterface<Country>, GenericSeriveImplemantation<Country>>();
builder.Services.AddScoped<IGenericServiceInterface<City>, GenericSeriveImplemantation<City>>();
builder.Services.AddScoped<IGenericServiceInterface<Town>, GenericSeriveImplemantation<Town>>();

builder.Services.AddScoped<IGenericServiceInterface<Overtime>, GenericSeriveImplemantation<Overtime>>();
builder.Services.AddScoped<IGenericServiceInterface<OvertimeType>, GenericSeriveImplemantation<OvertimeType>>();

builder.Services.AddScoped<IGenericServiceInterface<Vacation>, GenericSeriveImplemantation<Vacation>>();
builder.Services.AddScoped<IGenericServiceInterface<VacationType>, GenericSeriveImplemantation<VacationType>>();

builder.Services.AddScoped<IGenericServiceInterface<Sanciton>, GenericSeriveImplemantation<Sanciton>>();
builder.Services.AddScoped<IGenericServiceInterface<SancitonType>, GenericSeriveImplemantation<SancitonType>>();

builder.Services.AddScoped<IGenericServiceInterface<Doctor>, GenericSeriveImplemantation<Doctor>>();

// Employee
builder.Services.AddScoped<IGenericServiceInterface<Employee>, GenericSeriveImplemantation<Employee>>();

builder.Services.AddScoped<AllState>();

builder.Services.AddSyncfusionBlazor();
builder.Services.AddScoped<SfDialogService>();

await builder.Build().RunAsync();

