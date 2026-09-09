using Microsoft.EntityFrameworkCore;
using SubastaYa.API.Middlewares;
using SubastaYa.Application.Interfaces;
using SubastaYa.Application.Services;
using SubastaYa.Domain.Interfaces;
using SubastaYa.Infrastructure.Context;
using SubastaYa.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. Controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Base de datos (EF Core con SQL Server)
builder.Services.AddDbContext<SubastaYaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Inyección de Dependencias: Repositorios (Infrastructure)
builder.Services.AddScoped<IAuditoriaLogRepository, AuditoriaLogRepository>();
builder.Services.AddScoped<IBilleteraRepository, BilleteraRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IPujaRepository, PujaRepository>();
builder.Services.AddScoped<ISubastaRepository, SubastaRepository>();
builder.Services.AddScoped<ITransaccionLedgerRepository, TransaccionLedgerRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// 4. Inyección de Dependencias: Servicios de Negocio (Application)
builder.Services.AddScoped<IBilleteraService, BilleteraService>();
builder.Services.AddScoped<ISubastaService, SubastaService>();
builder.Services.AddScoped<IPujaService, PujaService>();

// 5. Configurar CORS para cuando conectemos el Frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// 6. Aplicar migraciones pendientes automáticamente al arrancar
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SubastaYaDbContext>();
    dbContext.Database.Migrate();
}

// 7. Middleware global para manejo de errores (400, 404, 409 Conflict)
app.UseMiddleware<ExceptionMiddleware>();

// 8. Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();