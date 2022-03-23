using System.Net;
using System.Security.Cryptography.X509Certificates;
using Beer.Core;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(k => k.Listen(IPAddress.Any, 5242));
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient<IConveyorBeltGateway, ConveyorBeltGateway>(opt =>
{
    opt.BaseAddress = new Uri("http://hopster.m07039.clients.dev.nrk.no/");
    opt.DefaultRequestHeaders.Add("apiKey", Config.ApiKey);
});
builder.Services.AddHttpClient<IBrewingGateway, BrewingGateway>(opt =>
{
    opt.BaseAddress = new Uri("http://hopster.m07039.clients.dev.nrk.no/");
    opt.DefaultRequestHeaders.Add("apiKey", Config.ApiKey);
});
builder.Services.AddHttpClient<IBottleShopGateway, BottleShopGateway>(opt =>
{
    opt.BaseAddress = new Uri("http://hopster.m07039.clients.dev.nrk.no/");
    opt.DefaultRequestHeaders.Add("apiKey", Config.ApiKey);
});
builder.Services.AddSingleton<Tappery>();
builder.Services.AddSingleton<Shippery>();


var app = builder.Build();





// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseMiddleware<ByobMiddleware>();
//app.UseMiddleware<Jean>();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();