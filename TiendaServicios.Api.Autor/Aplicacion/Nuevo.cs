using FluentValidation;
using MediatR;
using TiendaServicios.Api.Autor.Model;
using TiendaServicios.Api.Autor.Persitencia;

namespace TiendaServicios.Api.Autor.Aplicacion
{
	public class Nuevo
	{
		public class Ejecuta : IRequest
		{
			public string Nombre { get; set; }
			public string Apellido { get; set; }
			public DateTime FechaNacimiento { get; set; }
		}

		public class EjecutaValidacion: AbstractValidator<Ejecuta>
		{
			public EjecutaValidacion() { 
				RuleFor(x => x.Nombre).NotEmpty();
				RuleFor(x => x.Apellido).NotEmpty();
			}
		}

		public class Manejador : IRequestHandler<Ejecuta>
		{
			public readonly ContextAutor _contexto;

            public Manejador(ContextAutor contexto)
            {
				_contexto = contexto;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
			{
				var autorLibro = new AutorLibro
				{
					Nombre = request.Nombre,
					Apellido = request.Apellido,
					FechaNacimiento = TimeZoneInfo.ConvertTimeToUtc(request.FechaNacimiento),
					AutorLibroGuid = Guid.NewGuid().ToString()
				};

				_contexto.AutorLibro.Add(autorLibro);
				try
				{
					var valor = await _contexto.SaveChangesAsync();

					if (valor > 0)
					{
						return Unit.Value;
					}

					throw new Exception("No se pudo insertar el Autor del Libro");
				}
				catch (Exception e)
				{

					throw new Exception(e.Message);
				}
				

			}
		}

	}
}
