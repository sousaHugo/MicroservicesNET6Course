using Shopping.Aggregator.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpClient<ICatalogService, CatalogService>(options =>
{
    options.BaseAddress = new Uri(builder.Configuration["ApiSettings:CatalogUrl"]);
});

builder.Services.AddHttpClient<IOrderService, OrderService>(options =>
    options.BaseAddress = new Uri(builder.Configuration["ApiSettings:OrderingUrl"]));


builder.Services.AddHttpClient<IBasketService, BasketService>(options =>
    options.BaseAddress = new Uri(builder.Configuration["ApiSettings:BasketUrl"]));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
