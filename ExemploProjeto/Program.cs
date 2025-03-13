using ExemploProjeto.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configurar o Entity Framework Core para usar MySQL
builder.Services.AddDbContext<BibliotecaDBContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("BibliotecaDB"),
        new MySqlServerVersion(new Version(8, 0, 31)) // Ajuste conforme sua versão do MySQL
    )
);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
