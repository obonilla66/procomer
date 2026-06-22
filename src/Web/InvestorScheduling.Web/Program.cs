
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("CatalogApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:CatalogApi"]
        ?? throw new InvalidOperationException("Services:CatalogApi es requerido."));
});

builder.Services.AddHttpClient("AgendaApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:AgendaApi"]
        ?? throw new InvalidOperationException("Services:AgendaApi es requerido."));
});

builder.Services.AddHttpClient("PdfApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:PdfApi"]
        ?? throw new InvalidOperationException("Services:PdfApi es requerido."));
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
