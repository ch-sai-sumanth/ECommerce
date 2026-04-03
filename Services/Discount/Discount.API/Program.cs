using System.Reflection;
using Discount.API.Services;
using Discount.Application.Handlers;
using Discount.Core.Repositories;
using Discount.Infrastructure.Repositories;
using Discount.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

var assemblies = new Assembly[]
{
    Assembly.GetExecutingAssembly(),
    typeof(CreateDiscountHandler).Assembly
};

builder.Services.AddMediatR(
    cfg => cfg.RegisterServicesFromAssemblies(assemblies));

builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddGrpc();

//Database Settings
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));

// This tells the DI container: "When someone asks for a Grpc Logger, give them the default one"
builder.Services.AddSingleton<Grpc.Core.Logging.ILogger>(Grpc.Core.GrpcEnvironment.Logger);

var app = builder.Build();

app.MigrateDatabase();
app.UseRouting();
app.UseEndpoints(ep =>
{
    ep.MapGrpcService<DiscountService>();
});

app.Run();

