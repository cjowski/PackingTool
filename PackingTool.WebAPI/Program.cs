using PackingTool.WebAPI.Extensions;
using System.Text.Json.Serialization;

var appSettings = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureServices(services =>
{
    services
        .AddControllers()
        .AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
        );

    services.AddRepositories();
    services.AddServices();
    services.AddDbContext(
        appSettings.GetConnectionString("DefaultConnection")!
    );

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddSpaStaticFiles(configuration =>
    {
        configuration.RootPath = "wwwroot/dist";
    });
    
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseSpaStaticFiles();
app.UseRouting();
app.MapControllers();
app.UseEndpoints(endpoints => { });

app.UseSpa(spa =>
{
    spa.Options.SourcePath = "Client";
    if (app.Environment.IsDevelopment())
    {
        spa.UseProxyToSpaDevelopmentServer("http://localhost:4000");
    }
});

app.Run();
