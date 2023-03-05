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
    services.AddSwagger();
    services.AddSpaStaticFiles(configuration =>
    {
        configuration.RootPath = "wwwroot/dist";
    });
    
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.AddSwaggerUI();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseEndpoints(endpoints => { });

app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseSpaStaticFiles();
app.UseSpa(spa =>
{
    spa.Options.SourcePath = "Client";
    if (app.Environment.IsDevelopment())
    {
        spa.UseProxyToSpaDevelopmentServer("http://localhost:4000");
    }
});

app.Run();
