using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.Libros.Model;
using TiendaServicios.Api.Libros.Persistencia;

namespace TiendaServicios.Api.Libros.Aplicacion
{
	public class ConsultaFiltro
	{
		public class LibroUnico : IRequest<LibroMaterialDto> {
			public Guid? LibroGuid { get; set; }
		}

		public class Manejador : IRequestHandler<LibroUnico, LibroMaterialDto>
		{
			private readonly ContextoLibreria _contexto;
			private readonly IMapper _mapper;

            public Manejador(ContextoLibreria contexto,IMapper mapper)
            {
                _contexto = contexto;
				_mapper = mapper;
			}
            public async Task<LibroMaterialDto> Handle(LibroUnico request, CancellationToken cancellationToken)
			{
				var libros = await _contexto.LibreriaMaterial.ToListAsync();
				var libro = await _contexto.LibreriaMaterial.Where(x => x.LibreriaMaterialId == request.LibroGuid).FirstOrDefaultAsync();

				if (libro == null)
				{
					throw new Exception("No se encontró el libro solicitado");
				}

				var libroDto = _mapper.Map<LibreriaMaterial,LibroMaterialDto>(libro);
				return libroDto;
				
			}
		}
	}
}
