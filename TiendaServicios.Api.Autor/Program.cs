using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using TiendaServicios.Api.Autor.Aplicacion;
using TiendaServicios.Api.Autor.ManejadorRabbit;
using TiendaServicios.Api.Autor.Persitencia;
using TiendaServicios.Mensajeria.Email.NetEmail;
using TiendaServicios.RabbitMQ.Bus.BusRabbit;
using TiendaServicios.RabbitMQ.Bus.EventoQueue;
using TiendaServicios.RabbitMQ.Bus.Implement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//No es necesario agregar otra linea de RegisterValidators para otras clases, una vez declarado aqui, el proyecto ubicará todas las clases que usen AbstractValidator
builder.Services.AddControllers().AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());

//Agregando contexto para conexion base de datos
builder.Services.AddDbContext<ContextAutor>(options =>
{
	options.UseNpgsql(builder.Configuration.GetConnectionString("ConexionDatabase"));
});

//esta linea se declara una sola vez, por que despues ya .net ha reconocido que esta usando MediaTR y busca todo lo que implemente IRequest
builder.Services.AddMediatR(typeof(Nuevo.Manejador).Assembly);

//esta linea se declara una sola vez, por que despues ya .net ha reconocido que esta usando AutoMapper
builder.Services.AddAutoMapper(typeof(Consulta.Manejador));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddTransient<IEventoManejador<EmailEventoQueue>, EmailEventoManejador>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddSingleton<IRabbitEventBus, RabbitEventBus>(sp =>
{
	var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
	return new RabbitEventBus(sp.GetService<IMediator>(), scopeFactory);
});

builder.Services.AddTransient<EmailEventoManejador>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

var eventBus = app.Services.GetRequiredService<IRabbitEventBus>();
eventBus.Suscribe<EmailEventoQueue, EmailEventoManejador>();

app.Run();
