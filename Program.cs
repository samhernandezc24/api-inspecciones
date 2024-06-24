using API.Inspecciones.Persistence;
using API.Inspecciones.Services;
using API.Inspecciones.Utils;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Globalization;
using Workcube.JwtAutentication;

var builder = WebApplication.CreateBuilder(args);

// Inyecta las dependencias de JWT.
builder.Services.AddCustomJwtAuthentication();

string connectionString = builder.Environment.IsProduction() ? "Production" : "Development";

builder.Services.AddDbContext<Context>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString(connectionString), x => x.MigrationsHistoryTable("__EFMigrationsHistory", "inspeccion")));

// Registrar servicios.
builder.Services.AddScoped<CategoriasService, CategoriasService>();
builder.Services.AddScoped<CategoriasItemsService, CategoriasItemsService>();
builder.Services.AddScoped<FormulariosTiposService, FormulariosTiposService>();
builder.Services.AddScoped<InspeccionesFicherosService, InspeccionesFicherosService>();
builder.Services.AddScoped<InspeccionesService, InspeccionesService>();
builder.Services.AddScoped<InspeccionesCategoriasService, InspeccionesCategoriasService>();
builder.Services.AddScoped<InspeccionesEstatusService, InspeccionesEstatusService>();
builder.Services.AddScoped<InspeccionesTiposService, InspeccionesTiposService>();
builder.Services.AddScoped<UnidadesService, UnidadesService>();
builder.Services.AddScoped<UnidadesCapacidadesMedidadesService, UnidadesCapacidadesMedidadesService>();

// Agregar servicios al contenedor.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de los servicios de AutoMapper.
builder.Services.AddAutoMapper(typeof(Program));

var mapperConfig = new MapperConfiguration(m =>
{
    m.AddProfile(new MappingProfile());
});

var app = builder.Build();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Ficheros")), RequestPath = new PathString("/Ficheros")
});

// Configurar el lenguaje predeterminado para el manejo de formatos de fecha, etc.
var cultureInfo = new CultureInfo("es-MX");

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Configurar la politica CORS para permitir solicitudes desde cualquier origen.
app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true).AllowCredentials());

// Configurar el canal de peticiones HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
