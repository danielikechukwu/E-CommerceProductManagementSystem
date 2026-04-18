using E_CommerceProductManagementSystem.Data;
using E_CommerceProductManagementSystem.Repositories;
using E_CommerceProductManagementSystem.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register service
builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Disabled Camel Case in JSON serialization and deserialization
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

// Get Connection String
var connectionString = builder.Configuration.GetConnectionString("ECommerceDatabaseManagementSystem");

// Register DbContext with PostgreSQL
builder.Services.AddDbContext<ECommerceProductManagementSystemDbContext>(options =>
{
    options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
});

// Register service
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference();

    app.MapGet("/", () => Results.Redirect("/scalar/v1"));
}

app.UseHttpsRedirection();

app.Run();