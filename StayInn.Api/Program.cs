
using Microsoft.EntityFrameworkCore;
using StayInn.Infrastructure.Persistence;


// Compatibilidad de fechas para Postgres
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


var builder = WebApplication.CreateBuilder(args);


// Cargar .env y variables de entorno
if (builder.Environment.IsDevelopment())
{
    DotNetEnv.Env.Load();
}

builder.Configuration.AddEnvironmentVariables();


// Leer variables de conexión desde las variable de entorno
var user = Environment.GetEnvironmentVariable("DB_USER");
var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
var host = Environment.GetEnvironmentVariable("DB_HOST");
var database = Environment.GetEnvironmentVariable("DB_NAME");
var port = Environment.GetEnvironmentVariable("DB_PORT");
var key = Environment.GetEnvironmentVariable("JWT_KEY");
var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");


// Validar variables de entorno para la conexión
var variablesFaltantes = new List<string>();
if (string.IsNullOrWhiteSpace(user)) variablesFaltantes.Add("DB_USER");
if (string.IsNullOrWhiteSpace(password)) variablesFaltantes.Add("DB_PASSWORD");
if (string.IsNullOrWhiteSpace(host)) variablesFaltantes.Add("DB_HOST");
if (string.IsNullOrWhiteSpace(database)) variablesFaltantes.Add("DB_NAME");
if (string.IsNullOrWhiteSpace(port)) variablesFaltantes.Add("DB_PORT");

if (variablesFaltantes.Any())
{
    var menssage = $"Faltan variables de entorno de conexión a la base de datos: {string.Join(", ", variablesFaltantes)}";
    Console.Error.WriteLine(menssage);
}


// Contruir la cadena de conexión en formato PostgreSQL
var connectionString =
    $"Host={host};" +
    $"Port={port};" +
    $"Database={database};" +
    $"Username={user};" +
    $"Password={password};";

// Registrar ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});



builder.Services.AddControllers();
builder.Services.AddOpenApi();


var app = builder.Build();


// Comprobar la conexión a la base de datos
app.Lifetime.ApplicationStarted.Register(() =>
{
    try
    {
        // Crear un scope para obtener el DbContext
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Intentar conectar
        if (dbContext.Database.CanConnect())
        {
            Console.WriteLine("Conexión a PostgreSQL establecida correctamente.");
        }
        else
        {
            Console.WriteLine($"No se puedo establecer la conexión a la base de datos.");
        }
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Error al comprobar la conexión a la base de datos: {ex.Message}");
    }
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
