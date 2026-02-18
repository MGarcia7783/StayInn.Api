
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using StayInn.Api.Middleware;
using StayInn.Application.Interfaces.Persistence;
using StayInn.Application.Interfaces.Service;
using StayInn.Application.Mappings;
using StayInn.Application.Services;
using StayInn.Infrastructure.Persistence.Data;
using StayInn.Infrastructure.Persistence.Repositories;
using StayInn.Infrastructure.Services;


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
    throw new Exception(
        $"Faltan variables de entorno: {string.Join(", ", variablesFaltantes)}"
    );
}



// Contruir la cadena de conexión en formato PostgreSQL
var connectionString =
    $"Host={host};" +
    $"Port={port};" +
    $"Database={database};" +
    $"Username={user};" +
    $"Password={password};"; /*+
    $"SSL Mode=Require;" +             
    $"Trust Server Certificate=true;";*/

// Registrar ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});


// Obtener credenciales de Cloudinary
var cloudName = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME");
var apiKey = Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY");
var apiSecret = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET");


// Crear cuenta y cloudinary
var accont = new Account(cloudName, apiKey, apiSecret);
var cloudinary = new Cloudinary(accont) { Api = { Secure = true } };


// Registrar Cloudinary
builder.Services.AddSingleton(cloudinary);


// Registrar repositorios con sus interfaces
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IHabitacionRepository, HabitacionRepository>();
builder.Services.AddScoped<IAreaEsparcimientoRepository, AreaEsparciminetoRepository>();
builder.Services.AddScoped<IReservacionRepository, ReservacionRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();


// Registrar servicios con sus interfaces
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IImageStorageService, CloudinaryImageStorageService>();
builder.Services.AddScoped<IHabitacionService, HabitacionService>();
builder.Services.AddScoped<IAreaEsparcimientoService, AreaEsparcimientoService>();


// Registrar AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));


// Agregar controladores
builder.Services.AddControllers();


// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer(); // Detectar los endpoint de API
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "StayInn API",
        Version = "v1",
        Description = "API para gestión de hotel StayInn"
    });
});


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



// Registrar Middleware para excepciones globales
app.UseMiddleware<ExceptionMiddleware>();



// Configuración para entornos de desarrollo y produccon
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "AtayInn API v1");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
