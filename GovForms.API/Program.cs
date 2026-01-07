using GovForms.Engine.Data;
using GovForms.Engine.Services;
using GovForms.API.Integrations;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICitizenService, MinistryOfInteriorSimulator>();
// --- המתג החכם שלנו ---
// true  = בבית (משתמשים ב-SQL)
// false = בעבודה (משתמשים ב-Mock)
bool useSql = true; 

if (useSql)
{
    // 1. שליפת כתובת ה-SQL מההגדרות (מתוך ה-JSON)
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    // 2. רישום ה-Repository בעזרת פונקציית ייצור (Factory)
    // אנחנו אומרים ל-DI: "אל תנסה לנחש, הנה הסטרינג שצריך להעביר ל-AppRepository"
    builder.Services.AddScoped<IAppRepository>(sp => new AppRepository(connectionString!));
    
    Console.WriteLine("--> Using SQL Database");
}
else
{
    builder.Services.AddSingleton<IAppRepository, MockAppRepository>();
    Console.WriteLine("--> Using Mock (Memory) Database");
}
builder.Services.AddScoped<WorkflowService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();