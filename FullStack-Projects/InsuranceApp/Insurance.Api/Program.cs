using FluentValidation;
using FluentValidation.AspNetCore;
using Insurance.Api.Middleware;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Application.Interfaces.Services;
using Insurance.Application.Mappers;
using Insurance.Application.Services;
using Insurance.Application.Validators.Clients;
using Insurance.Infrastructure;
using Insurance.Infrastructure.Mappers;
using Insurance.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateClientDtoValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var connectionString = builder.Configuration.GetConnectionString("InsuranceDb");

builder.Services.AddDbContext<InsuranceDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddAutoMapper(
    _ => { },
    typeof(DtoMapperConfig),
    typeof(DaoMapperConfig)
);

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IGeographyService, GeographyService>();
builder.Services.AddScoped<IBuildingService, BuildingService>();

builder.Services.AddScoped<IGeographyRepository, GeographyRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IBuildingRepository, BuildingRepository>();
builder.Services.AddScoped<IClientIdentifierRepository, ClientIdentifierRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();
//app.UseAuthentication();


app.MapControllers();

app.Run();
