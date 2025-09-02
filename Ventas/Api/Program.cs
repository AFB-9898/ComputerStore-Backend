using Domain.Interfaces;
using Infraestructura.Repositorios;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ===== Add services =====
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ===== Configuración CORS =====
var frontEndOrigins = new[]
{
    "http://localhost:5173",
};

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocal", policy =>
    {
        policy.WithOrigins(frontEndOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // si usas cookies/credenciales
    });
});

// ===== Conexion Base de Datos =====
builder.Services.AddDbContext<BdContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("Infraestructura")
    )
);

// ===== Registro de Repositorios (DI) =====
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<ICarritoRepository, CarritoRepository>();
builder.Services.AddScoped<IOrdenRepository, OrdenRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ITecnicoRepository, TecnicoRepository>();
builder.Services.AddScoped<IServicioTecnicoRepository, ServicioTecnicoRepository>();
builder.Services.AddScoped<IInventarioRepository, InventarioRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IPagoRepository, PagoRepository>();


var app = builder.Build();

// ===== Pipeline =====

// Manejo de errores para producción
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}
else
{
    // En desarrollo activamos Swagger
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MiTiendaPC API V1");
        c.RoutePrefix = "swagger"; 
    });
}

// Redirección a HTTPS
app.UseHttpsRedirection();

app.UseCors("AllowLocal");

// Si usas autenticación: habilita middleware aquí (ej. JWT Bearer)
// app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
