using TicoBus.BL.Interfaces;
using TicoBus.BL.Services;
using TicoBus.DA.Repositories;
using Microsoft.EntityFrameworkCore;
using TicoBus.DA.Data;

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