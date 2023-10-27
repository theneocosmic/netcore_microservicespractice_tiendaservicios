using TiendaServicios.Api.Gateway.ImplementRemote;

namespace TiendaServicios.Api.Gateway.LibroRemote
{
	public class LibroModeloRemote
	{
		public Guid? LibreriaMaterialId { get; set; }
		public string Titulo { get; set; }
		public DateTime? FechaPublicacion { get; set; }
		public Guid? AutorLibro { get; set; }
		public AutorModeloRemote AutorData { get; set; }

	}
}
