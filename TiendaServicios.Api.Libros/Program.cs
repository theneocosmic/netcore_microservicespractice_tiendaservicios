using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TiendaServicios.Api.Libros.Aplicacion;
using TiendaServicios.Api.Libros.Persistencia;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ContextoLibreria>(opt =>
{
	opt.UseSqlServer(builder.Configuration.GetConnectionString("ConexionDB"));
});

//MediaTR configuracion
builder.Services.AddMediatR(typeof(Nuevo.Manejador).Assembly);

//esta linea se declara una sola vez, por que despues ya .net ha reconocido que esta usando AutoMapper
builder.Services.AddAutoMapper(typeof(Consulta.Manejador));

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
