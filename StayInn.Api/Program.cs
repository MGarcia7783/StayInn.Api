
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using StayInn.Api.Middleware;
using StayInn.Application.Interfaces.Persistence;
using StayInn.Application.Interfaces.Service;
using StayInn.Application.Mappings;
using StayInn.Application.Services;
using StayInn.Domain.Entities;
using StayInn.Infrastructure.Persistence.Data;
using StayInn.Infrastructure.Persistence.Repositories;
using StayInn.Infrastructure.Services;
using System.Security.Claims;
using System.Text;
using System.Text.Json;


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
    $"Password={password};" +
    $"SSL Mode=Require;" +             
    $"Trust Server Certificate=true;";

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


// Reglas de seguridad para la password y email
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// Registrar repositorios con sus interfaces
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IHabitacionRepository, HabitacionRepository>();
builder.Services.AddScoped<IAreaEsparcimientoRepository, AreaEsparciminetoRepository>();
builder.Services.AddScoped<IReservacionRepository, ReservacionRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();


// Registrar servicios con sus interfaces
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IImageStorageService, CloudinaryImageStorageService>();
builder.Services.AddScoped<IHabitacionService, HabitacionService>();
builder.Services.AddScoped<IAreaEsparcimientoService, AreaEsparcimientoService>();
builder.Services.AddScoped<IReservacionService, ReservacionService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();


// Configurar JWT Authentication
if (string.IsNullOrEmpty(key))
{
    throw new InvalidOperationException("La clave JWT no está configurada correctamente.");
}

// Configurar la autenticación
builder.Services.AddAuthentication
    (
        options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }
    ).AddJwtBearer(options =>
    {
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = true,
            ValidateAudience = true,
            RoleClaimType = ClaimTypes.Role,
            ValidIssuer = issuer,
            ValidAudience = audience
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    status = 401,
                    detail = "No autenticado. El token es inválido o no fue enviado."
                }));
            },

            OnForbidden = async context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    status = 403,
                    detail = "Acceso denegado. No tiene permisos para acceder a este recurso."
                }));
            }
        };
    });


// Registrar AutoMapper
builder.Services.AddAutoMapper(cgf => { }, typeof(MappingProfile).Assembly);


// Agregar controladores
builder.Services.AddControllers();


// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer(); // Detectar los endpoint de API
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "StayInn API",
        Version = "v1",
        Description = """
        #### **Plataforma Backend para Gestión Integral de Hotel.**

        **StayInn API** es una plataforma backend robusta y escalable diseñada para la gestión integral de hoteles. Proporciona una amplia gama de funcionalidades que permiten a los hoteles administrar sus operaciones diarias de manera eficiente, mejorar la experiencia del cliente y optimizar sus recursos.

        ---

        ### Módulos Principales:
        - **Gestión de Habitaciones**: Permite a los hoteles administrar sus habitaciones, incluyendo la creación, actualización y eliminación de habitaciones, así como la gestión de su disponibilidad y características.
        - **Gestión de Reservaciones**: Facilita la gestión de reservaciones, permitiendo a los hoteles crear, actualizar y cancelar reservaciones, así como gestionar la disponibilidad de habitaciones en tiempo real.
        - **Gestión de Áreas de Esparcimiento**: Permite a los hoteles administrar sus áreas de esparcimiento, incluyendo la creación, actualización y eliminación de áreas, así como la gestión de su disponibilidad y características.
        - **Gestión de Usuarios**: Facilita la gestión de usuarios, permitiendo a los hoteles crear, actualizar y eliminar usuarios, así como gestionar sus roles y permisos.
    
        ---

        #### Acceso a Endpoints Protegidos
        Algunos endpoints requieren autenticación. Utilice el botón **Authorize** en la interfaz de Swagger para ingresar su token JWT. El formato del token debe ser:

        ```
        Authorization: {token}
        ```

        ---

        """,

        Contact = new OpenApiContact
        {
            Name = "Mario García (Soporte Técnico)",
            Email = "mgrmairena@gmail.com"
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    /*** Configuración de seguridad para Swagger (JWT) ***/

    // 1. Definir el esquema de seguridad
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = """
        Token JWT requerido para acceder a los endpoints protegidos.
        
        Formato esperado: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
        
        """
    });

    // 2. Aplicar el esquema de seguridad a los endpoints
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference(referenceId: "Bearer",hostDocument: document),
            new List<string>()
        }
    });
});


// Configuración de CORS
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy.WithOrigins(
                "http://localhost:4200",    // Angular
                "http://localhost:3000"    // React
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
        }
        else
        {
            // Solo para desarrollo si no hay configuración
            policy.WithOrigins("https://stayinnresort.netlify.app")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
    });
});


builder.Services.AddOpenApi();


// Construir la aplicacion
var app = builder.Build();


// Registrar Middleware para excepciones globales
app.UseMiddleware<ExceptionMiddleware>();


// Configuración para entornos de desarrollo y produccon
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "AtayInn API v1");
});


app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger/index.html");
    return Task.CompletedTask;
});


// Redireccionar HTTP a HTTPS en producción
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}


app.UseCors("FrontendPolicy");

// Soporte para la autenticación
app.UseAuthentication();
app.UseAuthorization();


// Mapear controladores
app.MapControllers();


// Ejecutar la app
if (app.Environment.IsDevelopment())
{
    app.Run();
}
else
{
    var apiPort = Environment.GetEnvironmentVariable("PORT") ?? "8080";
    app.Run($"http://0.0.0.0:{apiPort}");
}
