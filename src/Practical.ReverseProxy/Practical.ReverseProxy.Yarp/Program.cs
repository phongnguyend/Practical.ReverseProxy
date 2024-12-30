namespace Practical.ReverseProxy.Yarp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add the reverse proxy to capability to the server
        var proxyBuilder = builder.Services.AddReverseProxy();

        // Initialize the reverse proxy from the "ReverseProxy" section of configuration
        proxyBuilder.LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

        var app = builder.Build();

        app.MapReverseProxy();
        app.Run();
    }
}