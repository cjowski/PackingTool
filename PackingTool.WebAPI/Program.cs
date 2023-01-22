var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureServices(services =>
{
    services.AddControllers();
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
