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
bool useSql = false; 

if (useSql)
{
    // המצב האמיתי (לבית)
    // שימי לב: זה יקרוס אם אין לך SQL מותקן, לכן זה בתוך ה-IF
    builder.Services.AddScoped<IAppRepository, AppRepository>();
    Console.WriteLine("--> Using SQL Database");
}
else
{
    // מצב פיתוח/עבודה (ללא דאטה בייס)
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