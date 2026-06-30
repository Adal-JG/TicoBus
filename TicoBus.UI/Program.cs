using Microsoft.EntityFrameworkCore;
using TicoBus.BL.Interfaces;
using TicoBus.BL.Services;
using TicoBus.DA.Data;
using TicoBus.DA.Repositories;
using TicoBus.UI.ApiClients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<DashboardRepository>();
builder.Services.AddScoped<CorreoService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ChoferRepository>();
builder.Services.AddScoped<IChoferService, ChoferService>();
builder.Services.AddScoped<PasajeroRepository>();
builder.Services.AddScoped<IPasajeroService, PasajeroService>();
builder.Services.AddScoped<RutaRepository>();
builder.Services.AddScoped<IRutaService, RutaService>();
builder.Services.AddScoped<UnidadRepository>();
builder.Services.AddScoped<IUnidadService, UnidadService>();
builder.Services.AddScoped<ViajeRepository>();
builder.Services.AddScoped<IViajeService, ViajeService>();
builder.Services.AddScoped<ReservaRepository>();
builder.Services.AddScoped<IReservaService, ReservaService>();

builder.Services.AddHttpClient();
builder.Services.AddHttpClient<AuthApiClient>();
builder.Services.AddHttpClient<ChoferesApiClient>();
builder.Services.AddHttpClient<PasajerosApiClient>();
builder.Services.AddHttpClient<RutasApiClient>();
builder.Services.AddHttpClient<UnidadesApiClient>();
builder.Services.AddHttpClient<ViajesApiClient>();
builder.Services.AddHttpClient<ViajesEnCursoApiClient>();
builder.Services.AddHttpClient<ViajesCanceladosApiClient>();
builder.Services.AddHttpClient<MisViajesApiClient>();
builder.Services.AddHttpClient<PerfilApiClient>();
builder.Services.AddHttpClient<DashboardApiClient>();


builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();