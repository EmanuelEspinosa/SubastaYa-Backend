using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

// Registrar el Background Worker para el cierre automático de subastas
builder.Services.AddHostedService<SubastaYa.API.BackgroundServices.AuctionClosingWorker>();

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
builder.Services.AddScoped<IAuthService, AuthService>(); 

// 5. Configurar Middleware de Autenticación con JWT Bearer
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "ClaveSecretaUltraSeguraParaSubastaYa2026!");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"] ?? "SubastaYaAPI",
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"] ?? "SubastaYaClient",
        ClockSkew = TimeSpan.Zero
    };
});

// 6. Configurar CORS para cuando conectemos el Frontend
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

// 7. Aplicar migraciones pendientes automáticamente al arrancar
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SubastaYaDbContext>();
    dbContext.Database.Migrate();
}

// 8. Middleware global para manejo de errores (400, 404, 409 Conflict)
app.UseMiddleware<ExceptionMiddleware>();

// 9. Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();