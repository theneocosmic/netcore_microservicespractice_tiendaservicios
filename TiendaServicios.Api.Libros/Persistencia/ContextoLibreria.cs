using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.Libros.Model;

namespace TiendaServicios.Api.Libros.Persistencia
{
	public class ContextoLibreria: DbContext
	{
        public ContextoLibreria(){}
        public ContextoLibreria(DbContextOptions<ContextoLibreria> options):base(options){}

		public virtual DbSet<LibreriaMaterial> LibreriaMaterial { get; set; }
	}
}
