using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using TiendaServicios.Api.Gateway.ImplementRemote;
using TiendaServicios.Api.Gateway.InterfaceRemote;
using TiendaServicios.Api.Gateway.MessageHandler;

var builder = WebApplication.CreateBuilder(args);

//YOU NEED THIS LINE, IN ORDER THAT .NET RECOGNIZE THE OCELOT ENDPOINTS CONFIGURATIONS
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Add services to the container.

//builder.Services.AddControllers();


//ADD OCELOT SERVICE
builder.Services.AddOcelot().AddDelegatingHandler<LibroHandler>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient("AutorService", config =>
{
	config.BaseAddress = new Uri(builder.Configuration["Services:Autor"]);
});

builder.Services.AddTransient<IAutorRemote, AutorRemote>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();

//ADD USING FOR OCELOT
app.UseOcelot().Wait();

app.MapControllers();

app.Run();
