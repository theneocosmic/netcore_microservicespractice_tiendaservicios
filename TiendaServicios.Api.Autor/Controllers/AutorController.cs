using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiendaServicios.Api.Autor.Aplicacion;
using TiendaServicios.Api.Autor.Model;

namespace TiendaServicios.Api.Autor.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AutorController : ControllerBase
	{
        private readonly IMediator _mediator;
        public AutorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<AutorDto>>> Autores(){
            return await _mediator.Send(new Consulta.ListaAutor());
        }

		[HttpGet("{id}")]
		public async Task<ActionResult<AutorDto>> GetAutorLibro(string id)
		{
			return await _mediator.Send(new ConsultaFiltro.AutorUnico {AutorGuid = id });
		}


		[HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await _mediator.Send(data);
        } 
    }
}
