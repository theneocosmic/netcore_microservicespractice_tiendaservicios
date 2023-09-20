using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using TiendaServicios.Api.Autor.Aplicacion;
using TiendaServicios.Api.Autor.Persitencia;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//No es necesario agregar otra linea de RegisterValidators para otras clases, una vez declarado aqui, el proyecto ubicará todas las clases que usen AbstractValidator
builder.Services.AddControllers().AddFluentValidation(cfg =>cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());

//Agregando contexto para conexion base de datos
builder.Services.AddDbContext<ContextAutor>(options => {
	options.UseNpgsql(builder.Configuration.GetConnectionString("ConexionDatabase"));
});

//esta linea se declara una sola vez, por que despues ya .net ha reconocido que esta usando MediaTR y busca todo lo que implemente IRequest
builder.Services.AddMediatR(typeof(Nuevo.Manejador).Assembly);

//esta linea se declara una sola vez, por que despues ya .net ha reconocido que esta usando AutoMapper
builder.Services.AddAutoMapper(typeof(Consulta.Manejador));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
