using Basket.API.GrpcServices;
using Basket.Application;
using Basket.Infrastructure;
using MassTransit;
using System.Reflection;
using static Discount.Gprc.DiscountService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options => { options.SuppressAsyncSuffixInActionNames = false; });
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddGrpcClient<DiscountServiceClient>
    (options =>
    {
        options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]);
    });

builder.Services.AddScoped<DiscountGrpcService>();

builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
    });
});
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
