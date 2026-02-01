using System.Reflection;
using DMS.ServiceDefaults;
using DMS.SharedKernel.Application;
using DMS.SharedKernel.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddInfrastructureBuilder(
    databaseConnectionName: "dms", 
    redisConnectionName: "cache");

Assembly[] moduleApplicationAssemblies = [
    DMS.Modules.Attendance.Application.AssemblyReference.Assembly,
    DMS.Modules.Classes.Application.AssemblyReference.Assembly,
    DMS.Modules.Fees.Application.AssemblyReference.Assembly,
    DMS.Modules.Organisation.Application.AssemblyReference.Assembly,
    DMS.Modules.Reporting.Application.AssemblyReference.Assembly,
    DMS.Modules.Students.Application.AssemblyReference.Assembly];

builder.Services.AddApplication(moduleApplicationAssemblies);

builder.Services.AddInfrastructureServices();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
