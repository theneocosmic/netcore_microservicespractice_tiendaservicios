using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.Libros.Model;
using TiendaServicios.Api.Libros.Persistencia;

namespace TiendaServicios.Api.Libros.Aplicacion
{
	public class Consulta
	{
		public class ListaLibros : IRequest<List<LibroMaterialDto>> { }

		public class Manejador : IRequestHandler<ListaLibros, List<LibroMaterialDto>>
		{
			private readonly ContextoLibreria _contexto;
			private readonly IMapper _mapper;
			public Manejador(ContextoLibreria contexto, IMapper mapper)
			{
				_contexto = contexto;
				_mapper = mapper;
			}

			public async Task<List<LibroMaterialDto>> Handle(ListaLibros request, CancellationToken cancellationToken)
			{
				var listaLibros = await _contexto.LibreriaMaterial.ToListAsync();
				return _mapper.Map<List<LibreriaMaterial>,List<LibroMaterialDto>>(listaLibros);
			}
		}
	}
}
