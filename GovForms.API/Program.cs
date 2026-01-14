using GovForms.Engine.Data;
using GovForms.Engine.Services;
using GovForms.Engine.Interfaces;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using GovForms.API.Validators;
var builder = WebApplication.CreateBuilder(args);

// הגדרת SQL בבית
builder.Services.AddDbContext<GovFormsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ApplicationValidator>());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// --- רישום ה-DI: כאן נפתרת שגיאה CS7036! --- [cite: 2026-01-08]
builder.Services.AddScoped<IAppRepository, AppRepository>(); 
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<WorkflowService>(); // השרת יזריק הכל לבד!
builder.Services.AddScoped<IExternalIntegrationService, PopulationRegistrySimulator>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
var app = builder.Build();
app.UseMiddleware<GovForms.API.Middleware.ExceptionMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();