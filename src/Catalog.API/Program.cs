using Catalog.Application;
using Catalog.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers(options => { options.SuppressAsyncSuffixInActionNames = false; });

builder.Services.AddInfrastructure();
builder.Services.AddApplication();
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
