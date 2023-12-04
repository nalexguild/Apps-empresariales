using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalProyect.Services;
var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de Entity Framework y conexi�n a la base de datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

// Configuraci�n del servicio IParticipanteService
builder.Services.AddScoped<IParticipanteService, ParticipanteServiceSQL>();
builder.Services.AddScoped<IConferenciaService, ConferenciaServiceSQL>();
builder.Services.AddScoped<IAsistenciaService, AsistenciaServiceSQL>();

builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

// Configuraci�n de migraciones y creaci�n de la base de datos al iniciar la aplicaci�n
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate(); // Aplica migraciones pendientes
        // Puedes agregar m�s l�gica de inicializaci�n de la base de datos aqu� si es necesario
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error al aplicar migraciones y/o inicializar la base de datos.");
    }
}

app.Run();
