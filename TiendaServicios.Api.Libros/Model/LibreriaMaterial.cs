namespace TiendaServicios.Api.Libros.Model
{
	public class LibreriaMaterial
	{
		public Guid? LibreriaMaterialId { get; set; }
		public string Titulo { get; set; }
		public DateTime? FechaPublicacion { get; set; }
		public Guid? AutorLibro { get; set; }

	}
}
