using TicoBus.BL.Services;
using TicoBus.UI.ApiClients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<CorreoService>();

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