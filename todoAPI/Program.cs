using System;
using Microsoft.EntityFrameworkCore;
using todoAPI.Models;
//using CsvToDatabaseLib;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient("YourHttpClientName", client =>
{
    client.Timeout = TimeSpan.FromMinutes(5); // 設置為 5 分鐘或其他適合的時間
});
builder.Services.AddDbContext<dbContext>(opt =>
    opt.UseSqlServer("Server=tcp:oneling0114.database.windows.net,1433;Initial Catalog=realtek;Persist Security Info=False;User ID=azureuser;Password=Taylor1213:);MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1");
    });

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

