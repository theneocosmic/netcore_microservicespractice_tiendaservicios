using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.Autor.Model;
using TiendaServicios.Api.Autor.Persitencia;

namespace TiendaServicios.Api.Autor.Aplicacion
{
	public class Consulta
	{
        
        public class ListaAutor : IRequest<List<AutorDto>> { }

		public class Manejador : IRequestHandler<ListaAutor, List<AutorDto>>
		{
			private readonly ContextAutor _contexto;
			private readonly IMapper _mapper;

			public Manejador(ContextAutor contexto, IMapper mapper)
			{
				_contexto = contexto;
				_mapper = mapper;
			}

			public async Task<List<AutorDto>> Handle(ListaAutor request, CancellationToken cancellationToken)
			{
				var autores = await _contexto.AutorLibro.ToListAsync();

				var autoresDto = _mapper.Map<List<AutorLibro>,List<AutorDto>>(autores);

				return autoresDto;
			}
		}
	}
}
