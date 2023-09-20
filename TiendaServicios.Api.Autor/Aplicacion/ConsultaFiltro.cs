using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using TiendaServicios.Api.Autor.Model;
using TiendaServicios.Api.Autor.Persitencia;

namespace TiendaServicios.Api.Autor.Aplicacion
{
	public class ConsultaFiltro
	{
		public class AutorUnico : IRequest<AutorDto>
		{
			public string AutorGuid { get; set; }
		}

		public class Manejador : IRequestHandler<AutorUnico, AutorDto>
		{
			private readonly ContextAutor _context;
			private readonly IMapper _mapper;
            public Manejador(ContextAutor context, IMapper mapper)
            {
                _context = context;
				_mapper = mapper;
            }
            public async Task<AutorDto> Handle(AutorUnico request, CancellationToken cancellationToken)
			{
				var autor = await _context.AutorLibro.Where(x => x.AutorLibroGuid == request.AutorGuid).FirstOrDefaultAsync();
				if(autor == null)
				{
					throw new Exception("No se encontró el autor");
				}
				var autorDto = _mapper.Map<AutorLibro, AutorDto>(autor);
				return autorDto;
			}
		}
	}
}
