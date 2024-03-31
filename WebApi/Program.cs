using Microsoft.AspNetCore.Http.Json;
using WebApi;
using WebApi.DataAccess;
using WebApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.Configure<JsonOptions>(options => options.SerializerOptions.Converters.Add(new JsonDecimalConverter()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapOrders();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.Run();

