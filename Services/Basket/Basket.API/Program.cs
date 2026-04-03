using System.Reflection;
using Basket.API.Settings;
using Basket.Application.GrpcService;
using Basket.Application.Handlers;
using Basket.Core.Repositories;
using Basket.Infrstructure.Repositories;
using Basket.Infrstructure.Settings;
using Discount.Grpc.Protos;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

//Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var assemblies = new Assembly[]
{
    Assembly.GetExecutingAssembly(), typeof(CreateShoppingCartHandler).Assembly
};

builder.Services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(assemblies));

//Options pattern
builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection("CacheSettings"));

//Redis
builder.Services.AddStackExchangeRedisCache((options) =>
{
    options.Configuration = builder.Configuration.GetSection("CacheSettings")
        .GetValue<string>("ConnectionString");
});

builder.Services.Configure<GrpcSettings>(builder.Configuration.GetSection("GrpcSettings"));

//Register Grpc client using Ioption
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(
    (sp, cfg) =>
    {
        var grpcSetting = sp.GetRequiredService<IOptions<GrpcSettings>>().Value;
        cfg.Address = new Uri(grpcSetting.DiscountUrl);

    });

//Grpc Service
builder.Services.AddScoped<DiscountGrpcService>();
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(
    (sp, cfg) =>
    {
        var grpcSetting = sp.GetRequiredService<IOptions<GrpcSettings>>().Value;
        cfg.Address = new Uri(grpcSetting.DiscountUrl);

    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
