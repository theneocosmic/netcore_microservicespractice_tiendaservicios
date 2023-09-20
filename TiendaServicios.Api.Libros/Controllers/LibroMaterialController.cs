using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiendaServicios.Api.Libros.Aplicacion;

namespace TiendaServicios.Api.Libros.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LibroMaterialController : ControllerBase
	{
		private readonly IMediator _mediator;

		public LibroMaterialController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<ActionResult<List<LibroMaterialDto>>> Libros()
		{
			return await _mediator.Send(new Consulta.ListaLibros());
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<LibroMaterialDto>> Libros(Guid? id)
		{
			return await _mediator.Send(new ConsultaFiltro.LibroUnico { LibroGuid = id });
		}

		[HttpPost]
		public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data) {
			return await _mediator.Send(data);
		}


    }
}
