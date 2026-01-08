using GovForms.Engine.Data;
using GovForms.Engine.Services;
using GovForms.Engine.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. הגדרת בסיס הנתונים - חיוני לפתרון השגיאה שקיבלת [cite: 2026-01-08]
builder.Services.AddDbContext<GovFormsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. רישום שירותי התשתית של ה-API
builder.Services.AddControllers(); // חובה כדי שה-API יזהה את ה-Controllers
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization(); 

// 3. רישום השירותים המקצועיים (Dependency Injection) [cite: 2025-12-30]
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IAppRepository, AppRepository>();
builder.Services.AddScoped<WorkflowService>();

var app = builder.Build();

// הגדרת ה-Pipeline (סדר הפעולות של השרת)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers(); // חובה כדי לחבר את הכתובות לפונקציות ב-Controller

app.Run();