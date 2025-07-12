using TestTaskForEnergomera.Kml;
using TestTaskForEnergomera.Routing;
using TestTaskForEnergomera.Services;
using TestTaskForEnergomera.Tools;

namespace TestTaskForEnergomera;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateSlimBuilder(args);

        builder.Services.Configure<Paths>(builder.Configuration.GetSection("Paths"));
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
            options.SerializerOptions.IncludeFields = true;
        });

        Paths? paths = builder.Configuration.Get<Paths>();

        builder.Services.AddSingleton<IFieldService>(new FieldService(builder.Configuration));

        var app = builder.Build();

        RouteGroupBuilder geoApi = app.MapGroup("/geo");

        geoApi.MapRoute(app.Services.GetRequiredService<IFieldService>());

        app.Run();
    }
}
