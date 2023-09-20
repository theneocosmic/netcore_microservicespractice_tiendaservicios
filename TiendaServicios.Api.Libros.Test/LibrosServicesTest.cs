using AutoMapper;
using GenFu;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaServicios.Api.Libros.Aplicacion;
using TiendaServicios.Api.Libros.Model;
using TiendaServicios.Api.Libros.Persistencia;
using Xunit;

namespace TiendaServicios.Api.Libros.Test
{
	public class LibrosServicesTest
	{
		//Generando data Fake
		private IEnumerable<LibreriaMaterial> ObtenerDataPrueba()
		{
			//Aqui configuramos como queremos llenar los datos para los datos de prueba, con Genfu
			A.Configure<LibreriaMaterial>()
				.Fill(x => x.Titulo).AsArticleTitle()
				.Fill(x => x.LibreriaMaterialId, () => { return Guid.NewGuid(); });

			//Aqui indicamos cuandos elementos queremos crear
			var lista = A.ListOf<LibreriaMaterial>(30);
			lista[0].LibreriaMaterialId = Guid.Empty;

			return lista;
		}

		private Mock<ContextoLibreria> CrearContexto()
		{
			//a continuación, indicamos que la clase LibreriaMaterial tiene que ser una clase de tipo entidad
			// al no estar trabajando diretamente con la BD necesitamos hacer esta configuracion
			//Esta congifuracion nos retorna una lista, pero no es factible para hacer filtros. Para poder lograrlo hay que agregar una nueva clase AsyncQueryProvider
			var dataPrueba = ObtenerDataPrueba().AsQueryable();
			var dbSet = new Mock<DbSet<LibreriaMaterial>>();
			dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(dataPrueba.Provider);
			dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Expression).Returns(dataPrueba.Expression);
			dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.ElementType).Returns(dataPrueba.ElementType);
			dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.GetEnumerator()).Returns(dataPrueba.GetEnumerator());

			//Se requiere crear dos clases que soporten los metodos asincronos que necesita la entidad libreria meterial para que sea posible retornar la lista de elementos

			dbSet.As<IAsyncEnumerable<LibreriaMaterial>>().Setup(x => x.GetAsyncEnumerator(new System.Threading.CancellationToken()))
				.Returns(new AsyncEnumerator<LibreriaMaterial>(dataPrueba.GetEnumerator()));

			//Se agrega el provider para que sea posible hacer filtros
			dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(new AsyncQueryProvider<LibreriaMaterial>(dataPrueba.Provider));

			var contexto = new Mock<ContextoLibreria>();
			contexto.Setup(x => x.LibreriaMaterial).Returns(dbSet.Object);
			return contexto;
		}


		[Fact]
		public async void GetLibroPorId()
		{

			var moqContexto = CrearContexto();
			var mapConfig = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(new MappingTest());
			});
			var mapper = mapConfig.CreateMapper();


			var request = new ConsultaFiltro.LibroUnico();
			request.LibroGuid = Guid.Empty;


			var manejador = new ConsultaFiltro.Manejador(moqContexto.Object,mapper);
			var libro = await manejador.Handle(request, new System.Threading.CancellationToken());

			Assert.NotNull(libro);

		}


		[Fact]
		public async void GetLibros()
		{

			var moqContexto = CrearContexto();
			var mapConfig = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(new MappingTest());
			});
			var mapper = mapConfig.CreateMapper();

			var manejador = new Consulta.Manejador(moqContexto.Object, mapper);

			var request = new Consulta.ListaLibros();

			var Lista = await manejador.Handle(request, new System.Threading.CancellationToken());

			Assert.True(Lista.Any());

		}

		[Fact]
		public async void GuardarLibro()
		{
			System.Diagnostics.Debugger.Launch(); 

			//Creamos base de datos en memoria
			var options = new DbContextOptionsBuilder<ContextoLibreria>()
				.UseInMemoryDatabase(databaseName: "BaseDatosLibro")
				.Options;

			var contexto = new ContextoLibreria(options);

			var request = new Nuevo.Ejecuta();
			request.Titulo = "Libro de Microservicio";
			request.AutorLibro = Guid.Empty;
			request.FechaPublicacion = DateTime.Now;

			var manejador = new Nuevo.Manejador(contexto);
			var resultado = await manejador.Handle(request,new System.Threading.CancellationToken());

			Assert.True(resultado != null);
		}
	}
}
