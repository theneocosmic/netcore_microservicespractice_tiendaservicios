using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.Autor.Model;

namespace TiendaServicios.Api.Autor.Persitencia
{
	public class ContextAutor:DbContext
	{
		public ContextAutor(DbContextOptions<ContextAutor> options):base(options) { }
		public DbSet<AutorLibro> AutorLibro { get; set; }
		public DbSet<GradoAcademico> GradoAcademico { get; set; }

	}
}
