using AutoMapper;
using TiendaServicios.Api.Libros.Model;

namespace TiendaServicios.Api.Libros.Aplicacion
{
	public class MappingProfile:Profile
	{
		public MappingProfile()
		{
			CreateMap<LibreriaMaterial, LibroMaterialDto>();
		}
	}
}
