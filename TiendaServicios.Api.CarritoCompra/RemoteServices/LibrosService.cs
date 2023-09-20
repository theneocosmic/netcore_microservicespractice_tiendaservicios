using System.Text.Json;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;
using TiendaServicios.Api.CarritoCompra.RemoteModel;

namespace TiendaServicios.Api.CarritoCompra.RemoteServices
{
	public class LibrosService : ILibrosService
	{ 
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ILogger<LibrosService> _logger;

        public LibrosService(IHttpClientFactory httpClientFactory, ILogger<LibrosService> logger)
        {
            _httpClientFactory = httpClientFactory;
			_logger = logger;
        }

		/// <summary>
		/// Call Microservice EndPoint Libros
		/// </summary>
		/// <param name="LibroId"></param>
		/// <returns></returns>
        public async Task<(bool resultado, LibroRemote Libro, string ErrorMessage)> GetLibro(Guid LibroId)
		{
			try
			{
				var cliente = _httpClientFactory.CreateClient("Libros");
				var response = await cliente.GetAsync($"api/LibroMaterial/{LibroId}");
				if (response.IsSuccessStatusCode)
				{
					var contenido = await response.Content.ReadAsStringAsync();
					var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
					var resultado = JsonSerializer.Deserialize<LibroRemote>(contenido,options);
					return (true, resultado,null);
				}
				return (false, null, response.ReasonPhrase);

			}
			catch (Exception e)
			{

				_logger?.LogError(e.ToString());
				return (false, null, e.Message);
			}
		}
	}
}
