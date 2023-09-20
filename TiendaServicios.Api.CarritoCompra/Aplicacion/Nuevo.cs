using FluentValidation;
using MediatR;
using TiendaServicios.Api.CarritoCompra.Models;
using TiendaServicios.Api.CarritoCompra.Persistencia;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
	public class Nuevo
	{
		public class Ejecuta : IRequest
		{
			public DateTime FechaCreacionSesion { get; set; }
			public List<string> ProductoLista { get; set; }
		}

		public class EjecutaValidacion : AbstractValidator<Ejecuta>
		{
            public EjecutaValidacion()
            {
				RuleFor(x => x.FechaCreacionSesion).NotEmpty();
            }
        }

		public class Manejador : IRequestHandler<Ejecuta>
		{
			private readonly CarritoContexto _contexto;
            public Manejador(CarritoContexto contexto)
            {
                _contexto = contexto;
            }

			public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
			{
				var carritoSesion = new CarritoSesion
				{
					FechaCreacion = request.FechaCreacionSesion
				};

				 _contexto.CarritoSesion.Add(carritoSesion);
				var value = await _contexto.SaveChangesAsync();

				if(value == 0)
				{
					throw new Exception("Errores en la insercion del carrito de compras");
				}

				int id = carritoSesion.CarritoSesionId;

				foreach (var producto in request.ProductoLista)
				{
					var detalleSesion = new CarritoSesionDetalle
					{
						FechaCreacion = DateTime.Now,
						CarritoSesionId = id,
						ProductoSeleccionado = producto
					};

					_contexto.CarritoSesionDetalle.Add(detalleSesion);				
				}

				value = await _contexto.SaveChangesAsync();

				if(value > 0)
				{
					return Unit.Value;
				}

				throw new Exception("Hubo problemas al intentar guardar el detalle del carrito de compras");


			}
		}
	}
}
