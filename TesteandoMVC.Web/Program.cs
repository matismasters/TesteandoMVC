var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

// Servicios existentes
builder.Services.AddScoped<TesteandoMVC.Web.Services.ISimpleService, TesteandoMVC.Web.Services.SimpleService>();

// Nuevo servicio simple para Test Doubles Demo
builder.Services.AddScoped<TesteandoMVC.Web.Services.ITestDoublesService, TesteandoMVC.Web.Services.TestDoublesService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

// Make the implicit Program class public so test projects can access it
namespace TesteandoMVC.Web
{
    public partial class Program { }
}