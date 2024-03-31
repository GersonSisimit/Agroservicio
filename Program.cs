using Agroservicio;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(
    options =>
    {
        options.IOTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
    }
    );



//inyeccion de dependencias
builder.Services.AddDbContext<Agroservicio.DbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Cadena"))
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var ContextoDB = scope.ServiceProvider.GetRequiredService<Agroservicio.DbContext>();
    ContextoDB.Database.Migrate();
}


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

app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
