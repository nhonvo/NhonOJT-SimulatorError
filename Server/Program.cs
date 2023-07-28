using System.Threading.RateLimiting;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.RateLimiting;
using Server;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

builder.Services.AddRateLimiter(p => p.AddFixedWindowLimiter(policyName: "rate", o =>
{
    o.PermitLimit = 3;
    o.Window = TimeSpan.FromSeconds(5);
    o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    o.QueueLimit = 2;
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(p => p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<RateLimitMiddleware>();

app.Run();
