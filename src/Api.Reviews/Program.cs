using Api.Reviews.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// EF Core (local dev)
builder.Services.AddDbContext<ReviewsDbContext>(opt =>
    opt.UseSqlite("Data Source=Data/reviews.db"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }

app.UseHttpsRedirection();
// app.UseAuthentication(); // enable when wiring JWT/B2C
app.UseAuthorization();

app.MapControllers();

app.Run();
