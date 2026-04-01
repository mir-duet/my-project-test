using Microsoft.EntityFrameworkCore;
using MyProject.API.Middleware;
using MyProject.Application.Interfaces;
using MyProject.Application.Mappings;
using MyProject.Application.Services;
using MyProject.Infrastructure.Persistence;
using MyProject.Infrastructure.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("MyProjectDb"));

// AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

// MediatR
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly));

// Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Services Layer
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<DuetGlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
