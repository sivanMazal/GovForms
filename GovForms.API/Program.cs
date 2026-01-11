using GovForms.Engine.Data;
using GovForms.Engine.Services;
using GovForms.Engine.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// הגדרת SQL בבית
builder.Services.AddDbContext<GovFormsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- רישום ה-DI: כאן נפתרת שגיאה CS7036! --- [cite: 2026-01-08]
builder.Services.AddScoped<IAppRepository, AppRepository>(); 
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<WorkflowService>(); // השרת יזריק הכל לבד!

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();