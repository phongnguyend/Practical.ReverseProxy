using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Practical.ReverseProxy.Ocelot.HttpMessageHandlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOcelot()
    .AddDelegatingHandler<DebuggingHandler>(true);

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseOcelot().Wait();

app.Run();
