using Api.Catalog.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext, use SQLite for local dev
builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseSqlite("Data Source=Data/catalog.db"));

// Add Controllers - This was missing!
builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add Swagger services for UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Add Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API.Catalog v1");
        options.RoutePrefix = "swagger"; // This makes Swagger UI available at /swagger
    });
}

// Only use HTTPS redirection when not running HTTP-only in development
if (!app.Environment.IsDevelopment() || builder.Configuration.GetValue<bool>("UseHttpsRedirection", true))
{
    app.UseHttpsRedirection();
}

// Map Controllers - This was missing!
app.MapControllers();

app.Run();
