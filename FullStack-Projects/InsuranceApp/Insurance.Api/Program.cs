using Insurance.Application.Interfaces.Repositories;
using Insurance.Application.Interfaces.Services;
using Insurance.Application.Mappers;
using Insurance.Application.Services;
using Insurance.Infrastructure;
using Insurance.Infrastructure.Mappers;
using Insurance.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var connectionString = builder.Configuration.GetConnectionString("InsuranceDb");

builder.Services.AddDbContext<InsuranceDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddAutoMapper(
    typeof(DtoMapperConfig),
    typeof(DaoMapperConfig)
);

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IGeographyService, GeographyService>();
builder.Services.AddScoped<IBuildingService, BuildingService>();

builder.Services.AddScoped<IGeographyRepository, GeographyRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IBuildingRepository, BuildingRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
//app.UseAuthentication();
//app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
