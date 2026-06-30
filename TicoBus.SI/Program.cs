using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TicoBus.BL.Interfaces;
using TicoBus.BL.Services;
using TicoBus.DA.Data;
using TicoBus.DA.Repositories;
using TicoBus.SI.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiKeyFilter>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ApiKeyFilter>();

builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<ChoferRepository>();
builder.Services.AddScoped<PasajeroRepository>();
builder.Services.AddScoped<RutaRepository>();
builder.Services.AddScoped<UnidadRepository>();
builder.Services.AddScoped<ViajeRepository>();
builder.Services.AddScoped<ReservaRepository>();
builder.Services.AddScoped<DashboardRepository>();

builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IChoferService, ChoferService>();
builder.Services.AddScoped<IPasajeroService, PasajeroService>();
builder.Services.AddScoped<IRutaService, RutaService>();
builder.Services.AddScoped<IUnidadService, UnidadService>();
builder.Services.AddScoped<IViajeService, ViajeService>();
builder.Services.AddScoped<IReservaService, ReservaService>();
builder.Services.AddScoped<CorreoService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("PermitirTodo");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();