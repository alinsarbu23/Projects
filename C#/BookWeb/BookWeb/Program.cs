var builder = WebApplication.CreateBuilder(args); /*creaza aplicatia si citeste setarile
 din appsetings pentru proprietatile proiectului (string-uri, variabil globale etc)*/
// Add services to the container.
builder.Services.AddControllersWithViews(); //adauga suport pentru MVC 

var app = builder.Build();  //construieste aplicatia cu serviciile rulate

// Configure the HTTP request pipeline. config firele de executie pe cererile http
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
// httpps://localhost/category/    get/ 3
//      domain name   controller action id  (index action by default)

app.Run();
