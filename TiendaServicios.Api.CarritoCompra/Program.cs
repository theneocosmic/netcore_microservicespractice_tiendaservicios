using MediatR;
using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.CarritoCompra.Aplicacion;
using TiendaServicios.Api.CarritoCompra.Persistencia;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;
using TiendaServicios.Api.CarritoCompra.RemoteServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ILibrosService, LibrosService>();

builder.Services.AddDbContext<CarritoContexto>(options =>
{
	options.UseMySQL(builder.Configuration.GetConnectionString("ConexionDatabase"));
});

//MediaTR configuracion
builder.Services.AddMediatR(typeof(Nuevo.Manejador).Assembly);

//esta linea se declara una sola vez, por que despues ya .net ha reconocido que esta usando AutoMapper
//builder.Services.AddAutoMapper(typeof(Consulta.Manejador));

builder.Services.AddHttpClient("Libros", config =>
{
	config.BaseAddress = new Uri(builder.Configuration["Services:Libros"]);
});

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
